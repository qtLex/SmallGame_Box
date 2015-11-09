using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EditorFunctionsUIBuilder : MonoBehaviour {

	public GameObject ButtonPrefab;
	private string[] MenuComands = {"Add","Delete","Move","Connect","New","Save"};
	private CubeGridEditorGameMode _editor;

	void Start(){
		_editor = GlobalOptions.GetEditorComponent();
	}

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
			EditorFunctionsButtonTag tagComponent = NewButton.AddComponent<EditorFunctionsButtonTag>();
			tagComponent.CommandName = s;
			NewButton.transform.SetParent(transform);
			Text ButtonText = NewButton.GetComponentInChildren<Text>();
			ButtonText.text = s;

		}


	}

	public void EnterMyWindows(){
		
		_editor.TraceMouse = false;

	}

	public void ExitMyWindows(){

		_editor.TraceMouse = true;

	}

	public void OnDisable(){

		_editor.TraceMouse = true;
	}

}
