using UnityEngine;
using System.Collections;

public class UIManager : MonoBehaviour {

	private GameObject ObjectCanvas;

	// Use this for initialization
	void Awake () {
		if (!ObjectCanvas)
			ObjectCanvas = GameObject.Find("Menu");
	}
	
	// Update is called once per frame
	void Update () {
		if(!ObjectCanvas)
			return;

		if (Input.GetKeyDown(KeyCode.Escape))
		{
			if(Time.timeScale != 0)
			{
				Time.timeScale = 0;
				Canvas menu = ObjectCanvas.GetComponent<Canvas>();
				menu.enabled = true;
				
			}
			else
			{				
				Time.timeScale = 1;
				Canvas menu = ObjectCanvas.GetComponent<Canvas>();
				menu.enabled = false; 
			}
		}
	}
}
