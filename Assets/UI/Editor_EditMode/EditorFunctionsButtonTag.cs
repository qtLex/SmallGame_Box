using UnityEngine;
using System.Collections;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class EditorFunctionsButtonTag : AnyButtonScript {

	public string CommandName;
	// "Add","Delete","Move","Connect","Open","Save","Exit"

	void Awake(){
		base.AwakeBase();
	}

	public override void onClick(){
			
		if(CommandName.Length == 0) return;

		switch (CommandName){
			case "Add": break;
			case "Delete": break;
			case "Move": break;
			case "Connect": break;
			case "Open": break;
			case "Save": break;

			default: break;
		}


			
	}

}
