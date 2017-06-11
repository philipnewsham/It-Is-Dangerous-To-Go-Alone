using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearProgress : MonoBehaviour 
{
	public GameController gameControllerScript;
	void Update () {
		if (Input.GetKeyDown (KeyCode.Delete)) {
			print ("DeletedStats!");
			DeleteProgress ();
		}
	}

	void DeleteProgress()
	{
		PlayerPrefs.SetInt ("AttackRange", 0);
		PlayerPrefs.SetInt ("DefenceRange", 0);
		PlayerPrefs.SetInt ("MagicRange", 0);
		PlayerPrefs.SetInt ("GoldLevel", 0);
		PlayerPrefs.SetInt ("QuestLevel", 0);
		gameControllerScript.Restart ();
	}
}
