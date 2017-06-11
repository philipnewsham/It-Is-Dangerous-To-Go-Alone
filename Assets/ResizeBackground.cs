using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ResizeBackground : MonoBehaviour 
{

	// Use this for initialization
	void Start () 
	{
		float height = Screen.height;
		float width = height;
		GetComponent<RectTransform> ().sizeDelta = new Vector2 (width, height);
	}

}
