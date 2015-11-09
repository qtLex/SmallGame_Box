using UnityEngine;
using System.Collections;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class DeadMenuButtonTag : AnyButtonScript {

	public string CommandName;
	
	void Awake(){
		base.AwakeBase();
	}
	
	public override void onClick(){
		
		if(CommandName.Length == 0) return;
		
		switch(CommandName){
		case "Restart":{

			LevelManager manager = GlobalOptions.LevelManager();
			manager.ReloadCurrentLevel();

			MenuManager menuManager = FindObjectOfType<MenuManager>() as MenuManager;
			menuManager.HideCurrentMenu();
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
