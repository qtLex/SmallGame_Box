using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class BoxSelectionUIBuilder : MonoBehaviour {

	public GameObject ButtonPrefab;
	private static CubeGridEditorGameMode _editor;
	private static CubeLibrary _library;
	private List<Cube> _cubeList;
	private Texture[] _previewArray;

	void Start(){

	}

	void Awake () {

		_editor = GlobalOptions.GetEditorComponent();
		_library = _editor.Library;
		_cubeList = _library.GetList();
   		_previewArray = _library.GetImageList();
		
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
		int _index = 0;
   		foreach(Cube _iteartor in _cubeList){

			GameObject NewButton = GameObject.Instantiate(ButtonPrefab) as GameObject;
			BoxSelectionButtonTag tagComponent = NewButton.AddComponent<BoxSelectionButtonTag>();
			tagComponent.Index = _index;
			tagComponent.uid = _iteartor.Key;
			NewButton.transform.SetParent(transform);
			Transform ButtonImage = NewButton.transform.Find("Button");
			//Text ButtonText = NewButton.GetComponentInChildren<Text>();
			//ButtonText.text = s;

        	Image Image = ButtonImage.gameObject.GetComponentInChildren<Image>();
			Texture2D tex2d = (Texture2D)_previewArray[_index];
			if (tex2d){
				Image.sprite = Sprite.Create(tex2d, new Rect(0,0,tex2d.width,tex2d.height), new Vector2(0.5f, 0.5f));
				Image.type = Image.Type.Simple;
			}

			_index ++;
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
