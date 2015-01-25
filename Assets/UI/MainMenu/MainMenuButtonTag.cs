using UnityEngine;
using System.Collections;
using UnityEngine.UI;

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
				Application.Quit();
				break;
			};

			default: break;
		}


			
	}

}
