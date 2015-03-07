using UnityEngine;
using System.Collections;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class MainMenuButtonTag : AnyButtonScript {

	public string CommandName;

	void Awake(){
		base.AwakeBase();
	}

	public override void onClick(){
			
		if(CommandName.Length == 0) return;

		switch (CommandName){
			case "Load":{
			MenuManager Manager = FindObjectOfType<MenuManager>();
			if (Manager == null) return;

				Manager.ShowMenu(1);
				break;
			};

			case "Exit":{
#if UNITY_EDITOR
				EditorApplication.isPlaying = false;
#else
				Application.Quit();
#endif
				break;
			};

			default: break;
		}


			
	}

}
