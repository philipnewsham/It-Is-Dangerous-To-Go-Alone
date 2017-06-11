using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BuyingPerks : MonoBehaviour 
{
	private int m_gold;
	public Button[] perkButtons; //0,1,2 (+1 to attack,defence,magic) - 3 (+1 to goldMultiplier) - 4 (buy out competition)
	private Text[] m_perkButtonTexts = new Text[3];
	private int m_attackLevel;
	private int m_defenceLevel;
	private int m_magicLevel;
	private int m_goldLevel;
	private GameController m_gameController;
	private int m_questLevel;
	private int m_questCount;

	private int[] m_statCosts = new int[3];
	void Start()
	{
		m_gameController = GetComponent<GameController> ();
		print ("ResetLevel");
		for (int i = 0; i < 3; i++) 
		{
			m_perkButtonTexts [i] = perkButtons [i].GetComponentInChildren<Text> ();
		}

		if (PlayerPrefs.HasKey ("AttackRange")) 
		{
			print ("haskey");
			m_attackLevel = PlayerPrefs.GetInt ("AttackRange");
		}
		if (PlayerPrefs.HasKey ("DefenceRange")) 
		{
			m_defenceLevel = PlayerPrefs.GetInt ("DefenceRange");
		}
		if (PlayerPrefs.HasKey ("MagicRange")) 
		{
			m_magicLevel = PlayerPrefs.GetInt ("MagicRange");
		}
		if (PlayerPrefs.HasKey ("GoldLevel")) 
		{
			m_goldLevel = PlayerPrefs.GetInt ("GoldLevel");
		}
		if (PlayerPrefs.HasKey ("QuestLevel")) 
		{
			m_questLevel = PlayerPrefs.GetInt ("QuestLevel");
		}
		if (PlayerPrefs.HasKey ("QuestCount")) 
		{
			m_questCount = PlayerPrefs.GetInt ("QuestCount");
		}

		UpdateCosts ();
	}

	void UpdateCosts()
	{
		m_statCosts [0] = (50 * m_attackLevel) + 250;
		m_perkButtonTexts [0].text = string.Format ("Upgrade Attack\n{0} Gold", m_statCosts [0]);
		m_statCosts [1] = (50 * m_defenceLevel) + 250;
		m_perkButtonTexts [1].text = string.Format ("Upgrade Defence\n{0} Gold", m_statCosts [1]);
		m_statCosts [2] = (50 * m_magicLevel) + 250;
		m_perkButtonTexts [2].text = string.Format ("Upgrade Magic\n{0} Gold", m_statCosts [2]);
	}

	public void RetrieveGold(int gold)
	{
		m_gold = gold;
		CheckButtons ();
	}

	void CheckButtons()
	{
		for (int i = 0; i < perkButtons.Length; i++) 
		{
			perkButtons [i].interactable = false;
		}

		if (m_gold >= m_statCosts [0]) 
		{
			perkButtons [0].interactable = true;
		}

		if (m_gold >= m_statCosts [1]) 
		{
			perkButtons [1].interactable = true;
		}

		if (m_gold >= m_statCosts [2]) 
		{
			perkButtons [2].interactable = true;
		}

		if (m_gold >= 1000) 
		{
			perkButtons [3].interactable = true;
		}

		if (m_gold >= 1000000) 
		{
			perkButtons [4].interactable = true;
		}
	}
	
	public void UpgradeAttack()
	{
		m_attackLevel += 1;
		PlayerPrefs.SetInt ("AttackRange", m_attackLevel);
		m_gameController.AddGold (- m_statCosts [0]);
		CheckToUpgradeQuest ();
	}

	public void UpgradeDefence()
	{
		m_defenceLevel += 1;
		PlayerPrefs.SetInt ("DefenceRange", m_defenceLevel);
		m_gameController.AddGold (- m_statCosts [1]);
		CheckToUpgradeQuest ();
	}

	public void UpgradeMagic()
	{
		m_magicLevel += 1;
		PlayerPrefs.SetInt ("MagicRange", m_magicLevel);
		m_gameController.AddGold (- m_statCosts [2]);
		CheckToUpgradeQuest ();
	}

	void CheckToUpgradeQuest()
	{
		m_questCount += 1;
		if (m_questCount >= 3) 
		{
			m_questCount -= 3;
			m_questLevel += 1;
			print ("questlevel increased to: " + m_questLevel);
			PlayerPrefs.SetInt ("QuestLevel", m_questLevel);
		}
		print("questCount = " + m_questCount);
		PlayerPrefs.SetInt ("QuestCount", m_questCount);

		UpdateCosts ();
	}

	public void UpgradeGold()
	{
		m_goldLevel += 1;
		PlayerPrefs.SetInt ("GoldLevel",m_goldLevel);
		m_gameController.AddGold (-1000);
	}

	public void BuyOut()
	{
		print ("you win?");
		m_gameController.AddGold (-1000000);
	}
}