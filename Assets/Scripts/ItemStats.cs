using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemStats : MonoBehaviour {
	public int attack;
	public int defence;
	public int magic;
	public string itemName;
	private Text m_text;
	public int itemID;
	private int m_attackRange;
	private int m_defenceRange;
	private int m_magicRange;
	private GameController m_gameController;


	private string[] m_names = new string[6]
	{
		"Sword",
		"Shield",
		"Wand",
		"Helmet",
		"Cloak",
		"Armour"
	};

	private string[] m_itemNames = new string[11]
	{
		"Battle Axe",
		"Hand Axe",
		"Hatchet",
		"Long Sword",
		"Short Sword",
		"Helmet",
		"Helmet",
		"Helmet",
		"Robes",
		"Robes",
		"Robes"
	};

	public Sprite[] itemSprites;
	public Image itemImage;
	/*
	private string m_defenceNames = new string[3] 
	{
		"Helmet",
		"Helmet",
		"Helmet"
	};

	private string m_magicNames = new string[3]
	{
		"Robes",
		"Robes",
		"Robes"
	};
	*/
	void Awake () 
	{
		m_text = GetComponentInChildren<Text> ();
		m_gameController = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController>();

		if (PlayerPrefs.HasKey ("AttackRange")) 
		{
			m_attackRange = PlayerPrefs.GetInt ("AttackRange");
		}
		if (PlayerPrefs.HasKey ("DefenceRange")) 
		{
			m_defenceRange = PlayerPrefs.GetInt ("DefenceRange");
		}
		if (PlayerPrefs.HasKey ("MagicRange")) 
		{
			m_magicRange = PlayerPrefs.GetInt ("MagicRange");
		}

		RandomiseStats ();
	}

	//randomises the three stats and adds a name
	void RandomiseStats()
	{
		int randomStat = Random.Range (0, 3);

		attack = Random.Range (m_attackRange, m_attackRange + 3);
		defence = Random.Range (m_defenceRange, m_defenceRange + 3);
		magic = Random.Range (m_magicRange, m_magicRange + 3);

		switch (randomStat) 
		{
		case 0:
			attack += 1;
			break;
		case 1:
			defence += 1;
			break;
		case 2:
			magic += 1;
			break;
		}

		//itemName = m_names [Random.Range (0, m_names.Length)];

		int randomNo = Random.Range (0, m_itemNames.Length);
		itemImage.sprite = itemSprites [randomNo];
		itemName = m_itemNames [randomNo];

		m_text.text = string.Format ("Attack: {0}, Defence: {1}, Magic: {2}", attack, defence, magic);
	}

	//when clicked on this button, sends information to GameController and makes un-interactable
	public void ChosenCard()
	{
		m_gameController.PlaceItem (itemName, attack, defence, magic, itemID);
		GetComponent<Button> ().interactable = false;
	}
}
