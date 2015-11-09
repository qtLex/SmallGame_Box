using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelButtonTag : AnyButtonScript {

	private LevelManager _manager;
	public LevelManager.Level LevelInfo;

	void Awake()
    {	
		base.AwakeBase();
		_manager = GlobalOptions.LevelManager();
	}

	// ived
	public override void onClick()
    {

		if(_manager == null) return;

		_manager.LoadByPath(LevelInfo.path);

		// выключаем меню
		MenuManager menuManager = FindObjectOfType<MenuManager>() as MenuManager;
		menuManager.HideCurrentMenu();
	}
}
