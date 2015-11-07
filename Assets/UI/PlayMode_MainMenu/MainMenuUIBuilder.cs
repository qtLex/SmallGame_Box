using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MainMenuUIBuilder : MonoBehaviour {

	public GameObject ButtonPrefab;
	private string[] MenuComands = {"Load", "Exit"};

	// Use this for initialization
	void Awake () {

		RebuildMenu();

	}

	void RebuildMenu(){

		// Очистка списка
		RectTransform[] transforms = GetComponentsInChildren<RectTransform>();
		foreach(RectTransform t in transforms){
			if (t.gameObject != this.gameObject){
				DestroyImmediate(t.gameObject);
			}
		};
	
		if(ButtonPrefab == null)return;

		foreach(string s in MenuComands){

			GameObject NewButton = GameObject.Instantiate(ButtonPrefab) as GameObject;
			MainMenuButtonTag tagComponent = NewButton.AddComponent<MainMenuButtonTag>();
			tagComponent.CommandName = s;
			NewButton.transform.SetParent(transform);
			Text ButtonText = NewButton.GetComponentInChildren<Text>();
			ButtonText.text = s;

		}


	}

	// Update is called once per frame
	void Update () {
	
	}
}
