using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DeadMenuButtonTag : AnyButtonScript {

	public string CommandName;
	
	void Awake(){
		base.AwakeBase();
	}
	
	public override void onClick(){
		
		if(CommandName.Length == 0) return;
		
		switch(CommandName){
		case "Restart":{
			GameObject EmptySingleton = GameObject.Find("Empty_Singleton");
			LevelManager manager = EmptySingleton.GetComponent<LevelManager>();
			manager.ReloadCurrentLevel();

			break;
		};
			
		case "Exit":{
			Application.Quit();
			break;
		};
			
		default: break;
		}
	}
}
