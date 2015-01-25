using UnityEngine;
using System.Collections;

public class UIAction : MonoBehaviour {

	private GameObject ObjectCanvas;
	
	// Use this for initialization
	void Awake () {
		if (!ObjectCanvas)
			ObjectCanvas = GameObject.Find("Menu");
	}

	public void UIActionReturnOnClick(){
		if(Time.timeScale == 1)
			return;

		Debug.Log("Продолжить");

		if(!ObjectCanvas)
			return;

		Time.timeScale = 1;
		Canvas menu = ObjectCanvas.GetComponent<Canvas>();
		menu.enabled = false;
	}

	public void UIActionNewGameOnClick(){
		if(Time.timeScale == 1)
			return;

		Debug.Log("Новая игра");
	}

	public void UIActionExitGameOnClick(){
		if(Time.timeScale == 1)
			return;

		Debug.Log("Выход");
		Application.Quit();
	}
}
