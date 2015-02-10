using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DeadMenuUIBuilder : MonoBehaviour {

	public GameObject ButtonPrefab;
	private string[] MenuComands = {"Restart", "Exit"};
	
	// Use this for initialization
	void Awake () {
		
		RebuildMenu();
		
	}
	
	void RebuildMenu(){
		
		// Очистка списка
		RectTransform[] transforms = GetComponentsInChildren<RectTransform>();
		foreach(RectTransform tran in transforms){
			if (tran.gameObject != this.gameObject){
				DestroyImmediate(tran.gameObject);
			}
		};
		
		if(ButtonPrefab == null)return;
		
		foreach(string comand in MenuComands){
			
			GameObject NewButton = GameObject.Instantiate(ButtonPrefab) as GameObject;
			DeadMenuButtonTag tagComponent = NewButton.AddComponent<DeadMenuButtonTag>();
			tagComponent.CommandName = comand;
			NewButton.transform.SetParent(transform);
			Text ButtonText = NewButton.GetComponentInChildren<Text>();
			ButtonText.text = comand;
			
		}
		
		
	}
}
