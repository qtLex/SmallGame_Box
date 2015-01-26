using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelButtonTag : AnyButtonScript {

	private LevelManager Manager;
	public LevelManager.Level LevelInfo;

	void Awake(){	
		base.AwakeBase();
		Manager = FindObjectOfType<LevelManager>() as LevelManager;
	}

	// ived
	public override void onClick(){

		if(Manager == null) return;

		Manager.LoadByPath(LevelInfo.path);

	}
}
