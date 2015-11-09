using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelUIBuilder : MonoBehaviour {
	
	public GameObject ButtonPrefab;
	private LevelManager Manager;
	
	void Awake()
    {
		RebuildGrid();
	}
	
	public void RebuildGrid(){

		Manager = GlobalOptions.LevelManager();

		// Очистка списка
		RectTransform[] transforms = GetComponentsInChildren<RectTransform>();
		foreach(RectTransform t in transforms){
			if (t.gameObject != this.gameObject){
				DestroyImmediate(t.gameObject);
			}
		};

		if(Manager == null || ButtonPrefab == null){
			return;
		}

		// Создание новых кнопок
		int i = 0;
		foreach(LevelManager.Level level in Manager.LevelList){

			GameObject NewButton = GameObject.Instantiate(ButtonPrefab) as GameObject;
			LevelButtonTag tagComponent = NewButton.AddComponent<LevelButtonTag>();
			tagComponent.LevelInfo = level;
			NewButton.transform.SetParent(transform);
			Text ButtonText = NewButton.GetComponentInChildren<Text>();
			ButtonText.text = (++i).ToString();
		}
				
	}
	
}
