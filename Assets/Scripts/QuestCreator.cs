using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class QuestCreator : MonoBehaviour {

	private int m_attack;
	private int m_defence;
	private int m_magic;
	private int m_questLevel;
	public int[] questStats = new int[3];
	public Text questStatText;
	public Text[] individualStatText;
	public Image monsterImage;
	public Text monsterDescription;
	public Text questTitle;
	private string[] m_enemyNames = new string[10] {
		"Wizard",
		"Wolf",
		"Croc",
		"Frank's Monster",
		"Zombie",
		"Sea Urchin",
		"Golem",
		"Harpy",
		"Cyclops",
		"Devil"
	};

	public Sprite[] enemySprites = new Sprite[10];

	private string[] m_enemyDescriptions = new string[10] 
	{
		"Wizard. Flings spells right at you, better duck because they're coming right at you.",
		"Werewolf in permanent wolf mode. Has tried to adapt to society but is evidently vilified as you can see by the fact it's on this quest card.",
		"Dentist. Why is that weird? Crocodiles have great teeth.",
		"Frank is a bit weird not just because he's an abomination of multiple corpses crudely spliced together but also because he eats ramen dry straight out the packet.",
		"This person crawled out from a grave somewhere and is absolutely fuming right now.",
		"Insecure fish person, hasn't been happy for a long time. Does that mean it has to die?",
		"Been working hard for years with no promotion, recently made redundant, now on the quest card list. Not going well really.",
		"Migrates south in the winter. Really nice pan fried, finish with a red wine jus.",
		"Money grabbing business bastard.",
		"It's a devil. Is it THE devil? Who cares?"
	};

	public Sprite[] characterSprites;
	public Image characterImage;

	void Start () 
	{
		if (PlayerPrefs.HasKey ("QuestLevel")) {
			m_questLevel = PlayerPrefs.GetInt ("QuestLevel");
		}
		QuestStats ();
	}
	
	// Update is called once per frame
	public void QuestStats () 
	{
		int enemyNo = Random.Range (0, enemySprites.Length);
		questTitle.text = string.Format("Slay the <color=red>{0}</color>",m_enemyNames[enemyNo]);
		monsterImage.sprite = enemySprites [enemyNo];
		monsterDescription.text = m_enemyDescriptions [enemyNo];
		characterImage.sprite = characterSprites [Random.Range (0, characterSprites.Length)];
		int mainStat = Random.Range (0, 3);
		m_attack = Random.Range (m_questLevel, m_questLevel + 5);
		m_defence = Random.Range (m_questLevel, m_questLevel + 5);
		m_magic = Random.Range (m_questLevel, m_questLevel + 5);

		switch (mainStat) 
		{
		case 0:
			m_attack += 1;
			break;
		case 1:
			m_defence += 1;
			break;
		case 2:
			m_magic += 1;
			break;
		}
		questStatText.text = string.Format ("<color=red>Attack: {0}</color> <color=blue>Defence: {1}</color> <color=green>Magic: {2}</color>", m_attack, m_defence, m_magic);
		individualStatText [0].text = string.Format ("<color=red>{0}</color>", m_attack);
		individualStatText [1].text = string.Format ("<color=blue>{0}</color>", m_defence);
		individualStatText [2].text = string.Format ("<color=green>{0}</color>", m_magic);
		questStats [0] = m_attack;
		questStats [1] = m_defence;
		questStats [2] = m_magic;
	}
}
