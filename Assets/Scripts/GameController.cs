using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameController : MonoBehaviour 
{
	public Button[] inventoryButtons;
	private ItemStats[] m_itemStats;
	private string[] m_itemNames = new string[3];
	private int[] m_attacks = new int[3];
	private int[] m_defences = new int[3];
	private int[] m_magics = new int[3];
	private int[] m_itemIDs = new int[3]{-1,-1,-1};
	private bool[] m_slotsFilled = new bool[3];
	private int m_currentSlotsFilled;
	private int m_currentSlot = 0;
	public Button[] characterButtons;
	private Text[] m_characterButtonsText = new Text[3];

	private int m_mainAttack;
	private int m_mainDefence;
	private int m_mainMagic;

	public GameObject blockingPanel;
	public Text currentStatsText;

	public int[] itemStats = new int[3];
	private QuestCreator m_questCreatorScript;
	public GameObject endOfTurnPanel;

	private int m_gold;
	public Text goldText;
	private BuyingPerks m_buyingPerkScript;
	public Button goButton;

	QuestCompletionCalculation m_questCompletionScript;

	private AudioSource m_audioSource;
	public AudioClip[] sfx; // 0 click, 1 door
	void Start () 
	{
		m_audioSource = GetComponent<AudioSource> ();
		m_questCompletionScript = GetComponent<QuestCompletionCalculation> ();
		if (PlayerPrefs.HasKey ("Gold")) 
		{
			m_gold = PlayerPrefs.GetInt ("Gold");
		}
		m_buyingPerkScript = GetComponent<BuyingPerks> ();
		UpdateGoldText ();
		m_questCreatorScript = GetComponent<QuestCreator> ();
		m_itemStats = new ItemStats[inventoryButtons.Length];
		for (int i = 0; i < inventoryButtons.Length; i++) 
		{
			m_itemStats [i] = inventoryButtons [i].GetComponentInChildren<ItemStats>();
			m_itemStats [i].itemID = i;
		}

		for (int i = 0; i < 3; i++) 
		{
			m_characterButtonsText [i] = characterButtons [i].GetComponentInChildren<Text> ();
		}
	}

	public void PlaceItem(string name, int attack, int defence, int magic, int itemID)
	{
		m_audioSource.clip = sfx [0];
		m_audioSource.Play ();
		for (int i = 0; i < 3; i++) 
		{
			if (!m_slotsFilled[i]) 
			{
				m_currentSlot = i;
				break;
			} 
		}

		m_currentSlotsFilled += 1;
		goButton.interactable = true;
		if (m_currentSlotsFilled == 3) 
		{
			blockingPanel.SetActive (true);
		} 
		else 
		{
			blockingPanel.SetActive (false);
		}

		m_itemNames [m_currentSlot] = name;
		m_attacks [m_currentSlot] = attack;
		m_defences [m_currentSlot] = defence;
		m_magics [m_currentSlot] = magic;
		m_itemIDs [m_currentSlot] = itemID;

		characterButtons [m_currentSlot].interactable = true;
		characterButtons [m_currentSlot].GetComponentInChildren<Text> ().text = string.Format ("{0}\n<color=red>A: {1}</color> <color=blue>D: {2}</color> <color=green>M: {3}</color>", name, attack, defence, magic);
		m_slotsFilled [m_currentSlot] = true;

		UpdateStats ();
	}

	public void UnselectItem(int slotNo)
	{
		m_audioSource.clip = sfx [0];
		m_audioSource.Play ();
		m_attacks [slotNo] = 0;
		m_defences [slotNo] = 0;
		m_magics [slotNo] = 0;

		inventoryButtons [m_itemIDs [slotNo]].interactable = true;
		characterButtons [slotNo].interactable = false;
		m_characterButtonsText [slotNo].text = "";
		m_slotsFilled [slotNo] = false;
		m_itemIDs [slotNo] = -1;
		m_currentSlotsFilled -= 1;
		if (m_currentSlotsFilled == 0)
			goButton.interactable = false;
		blockingPanel.SetActive (false);
		UpdateStats ();
	}

	void UpdateStats()
	{
		m_mainAttack = 0;
		m_mainDefence = 0;
		m_mainMagic = 0;

		for (int i = 0; i < 3; i++) 
		{
			m_mainAttack += m_attacks [i];
			m_mainDefence += m_defences [i];
			m_mainMagic += m_magics [i];
		}
		currentStatsText.text = string.Format ("<color=red>Attack: {0}</color> <color=blue>Defence: {1}</color> <color=green>Magic: {2}</color>", m_mainAttack, m_mainDefence, m_mainMagic);

		itemStats [0] = m_mainAttack;
		itemStats [1] = m_mainDefence;
		itemStats [2] = m_mainMagic;
		CalculateStatPercentage ();
	}

	private int[] m_questStats = new int[3];
	private  int m_itemStatSum;
	private float[] m_statPercentages = new float[3];
	private int m_questStatSum;
	private float m_percentageChance;
	private int m_checkStats;
	public Image fillBar;
	public Color32[] barColours;
	void CalculateStatPercentage()
	{
		m_questStats = m_questCreatorScript.questStats;
		m_questStatSum = m_questStats[0] + m_questStats[1] + m_questStats[2];
		m_itemStatSum = itemStats [0] + itemStats [1] + itemStats [2];
		m_checkStats = 0;
		for (int i = 0; i < 3; i++) 
		{
			if (itemStats [i] >= m_questStats [i]) 
			{
				m_checkStats += 1;
			}
		}

		if (m_checkStats < 3) 
		{
			m_percentageChance = 20;
		}
		else
		{
			m_percentageChance = 60f / m_questStatSum;
			m_percentageChance *= m_itemStatSum;
		}
		fillBar.fillAmount = m_percentageChance / 100f;
		if (m_percentageChance >= 60) {
			fillBar.color = barColours [0];
		} else {
			fillBar.color = barColours [1];
		}
		//print (m_percentageChance);
	}
	private bool m_isStillTurn;
	//After completing a quest, check to see if it's still the turn
	public void NextQuest()
	{
		for (int i = 0; i < 3; i++) 
		{
			characterButtons [i].interactable = false;
		}
		//checks to see if there's still cards in play
		for (int i = 0; i < inventoryButtons.Length; i++) 
		{
			if (inventoryButtons [i].interactable) 
			{
				m_isStillTurn = true;
			}
		}

		if (m_isStillTurn) 
		{
			m_audioSource.clip = sfx [1];
			m_audioSource.Play ();
			m_questCreatorScript.QuestStats ();
			blockingPanel.SetActive (false);
			for (int i = 0; i < 3; i++) 
			{
				m_slotsFilled [i] = false;
				m_attacks [i] = 0;
				m_defences [i] = 0;
				m_magics [i] = 0;
				characterButtons [i].GetComponentInChildren<Text> ().text = "";
			}
			m_currentSlotsFilled = 0;

			UpdateStats ();
		}
		else
		{
			endOfTurnPanel.SetActive (true);
			AddGold (m_questCompletionScript.tempGold);
		}
		m_isStillTurn = false;
	}

	public void ResetScene()
	{
		SceneManager.LoadScene(1);
	}

	public void AddGold(int addedGold)
	{
		m_gold += addedGold;
		PlayerPrefs.SetInt ("Gold", m_gold);
		m_buyingPerkScript.RetrieveGold (m_gold);
		UpdateGoldText ();
	}

	public void Restart()
	{
		AddGold (-m_gold);
		ResetScene ();
	}

	void UpdateGoldText()
	{
		goldText.text = string.Format ("Gold: {0}", m_gold);
	}
}