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

	void Awake(){
		base.AwakeBase();
		if (!_editor){
			_editor = GlobalOptions.GetEditorComponent();
		}
	}

	public override void onClick(){

		CubeLibrary lib =  _editor.Library;
		_editor.CurrentObject = lib.GetGameObjectByIndex(Index);
			
	}

}
