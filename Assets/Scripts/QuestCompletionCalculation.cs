using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestCompletionCalculation : MonoBehaviour 
{
	private int[] m_questStats = new int[3];
	private  int[] m_itemStats = new int[3];
	private float[] m_statPercentages = new float[3];
	private int m_questStatSum;
	private int m_itemStatSum;
	private float m_percentageChance;
	public GameController gameControllerScript;
	public QuestCreator questCreatorScript;
	private int m_checkStats;
	private int m_gold;
	private int m_goldMultiplier;
	private int m_goldLevel;
	public int tempGold;
	void Start()
	{
		if (PlayerPrefs.HasKey ("GoldLevel")) 
		{
			m_goldLevel = PlayerPrefs.GetInt ("GoldLevel");
		}
	}
	//checks if items are over quest stats (if not, cap % at 40)
	void CalculateStatPercentage()
	{
		m_questStatSum = m_questStats[0] + m_questStats[1] + m_questStats[2];
		m_goldMultiplier = 25 + m_goldLevel;  
		m_gold = m_questStatSum * m_goldMultiplier;
		m_itemStatSum = m_itemStats [0] + m_itemStats [1] + m_itemStats [2];
		m_checkStats = 0;
		for (int i = 0; i < 3; i++) 
		{
			if (m_itemStats [i] >= m_questStats [i]) 
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

		print (m_percentageChance);
		RollDice ();
	}

	public void RollDice()
	{
		int rollDice = Random.Range (1, 101);
		print (rollDice);
		if (rollDice <= m_percentageChance) {
			print ("successful!");
			tempGold += m_gold;
			//gameControllerScript.AddGold (m_gold);
		} 
		else 
		{
			print ("failed!");
			tempGold += 10;
			//gameControllerScript.AddGold (10);
		}
		gameControllerScript.NextQuest ();
	}
	//grabs attack, defence, and magic for the quest and the items selected
	public void UpdateStats()
	{
		m_questStats = questCreatorScript.questStats;
		m_itemStats = gameControllerScript.itemStats;
		CalculateStatPercentage ();
	}
}
