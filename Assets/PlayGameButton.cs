using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayGameButton : MonoBehaviour {
    public string LevelName;
    public Animator SignFlip;
	public Animator Crowd;
	public Animator FadeOut;
    public int check;

    public void ClickButton()
    {
        SignFlip.SetTrigger("StartGame");
		Crowd.SetTrigger ("Go");
		FadeOut.SetTrigger ("Click");
        Invoke("LoadLevel", 5f);

    }
    void LoadLevel()
    {
        SceneManager.LoadScene(1);
        print("ClickedOn");
    }

	public void Quit()
	{
		Application.Quit ();
	}
}

