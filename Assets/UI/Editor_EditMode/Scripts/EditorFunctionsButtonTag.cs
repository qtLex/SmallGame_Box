using UnityEngine;
using System.Collections;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class EditorFunctionsButtonTag : AnyButtonScript {


	private static GameObject _currentButton;
	private static Image _currentImage;

	private static EditorFunctionsUIBuilder _builder;
	private static CubeGridEditorGameMode _editor;

	//private Image _thisImage;
	public string CommandName;
	// "Add","Delete","Move","Connect","Open","Save","Exit"

	void Start()
    {

		if (!_builder){
			_builder = GetComponentInParent<EditorFunctionsUIBuilder>();
		}
		if(!_editor){
			_editor = GlobalOptions.GetEditorComponent();
		}
	}

	void Awake(){
		base.AwakeBase();
	}

	public override void onClick(){
			
		if(CommandName.Length == 0) return;

		switch (CommandName){

		case "Add":    AddButtonPress(); break;
		case "Delete": DeleteButtonPress(); break;
		case "Move":   MoveButtonPress(); break;
		case "Connect":ConnectButtonPress(); break;
		case "Open":   OpenButtonPress(); break;
		case "Save":   SaveButtonPress(); break;

			default: break;
		}
	
	}

	#region FunctionRoutines
	private void AddButtonPress(){

		_editor.SetAddMode();
		AnyButtonPress();
	}
	private void DeleteButtonPress(){
		_editor.SetDeleteMode();
		AnyButtonPress();
	}
	private void MoveButtonPress(){
		_editor.SetMoveMode();
		AnyButtonPress();
	}
	private void ConnectButtonPress(){
		_editor.SetConnectMode();
		AnyButtonPress();
	}
	private void OpenButtonPress()
    {
        MenuManager menuManager = FindObjectOfType<MenuManager>() as MenuManager;

        if(!menuManager)
            return;

        menuManager.ShowMenu(1);
	}
	private void SaveButtonPress()
    {
        MenuManager menuManager = FindObjectOfType<MenuManager>() as MenuManager;

        if (!menuManager)
            return;
        menuManager.ShowMenu(4);
	}

	private void AnyButtonPress()
    {
		if (_currentButton){
			_currentImage = _currentButton.GetComponent<Image>();
			_currentImage.color = _builder.ButtonPrefab.GetComponent<Image>().color;
		}

		_currentButton = this.gameObject;

		_currentImage = _currentButton.GetComponent<Image>();
		_currentImage.color = Color.green;
	}

	#endregion

}
