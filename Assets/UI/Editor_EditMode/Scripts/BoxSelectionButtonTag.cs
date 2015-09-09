using UnityEngine;
using System.Collections;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class BoxSelectionButtonTag : AnyButtonScript {

	public int Index;
	public string uid;
	public static CubeGridEditorGameMode _editor;
	private static Image _currentButton;
	private Image _buttonImage;
	private static BoxSelectionUIBuilder _uiBuilder;

	public Image ButtonImage{
		get{return _buttonImage;}
	}

	void Start(){
		if(!_buttonImage){
			Transform button = transform.FindChild("Button");
			if (button){
				_buttonImage = button.gameObject.GetComponent<Image>();
			}
		}

		if (!_uiBuilder){
			_uiBuilder = FindObjectOfType<BoxSelectionUIBuilder>();
		}

		if (!_editor){
			_editor = GlobalOptions.GetEditorComponent();
		}
	}

	void Awake(){
		base.AwakeBase();
	}

	public override void onClick(){

		if (_currentButton){
			_currentButton.color = Color.white;
		}
		
      	_editor.BrushObjectIndex = Index;
	  	_currentButton = this._buttonImage;
		_currentButton.color = Color.green;
	}

}
