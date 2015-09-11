using UnityEngine;
using System;
using System.Collections;
using GameEnums;

public static class GlobalOptions
{

	private static GameObject _Player;
	private static GameObject _CurrentBox;
	private static CubeGrid   _Grid;
	private static DeferredExecution _DeferredExecutionComponent;
	private static GameModes GameMode = GameEnums.GameModes.PlayMode;
	private static CubeGridEditorGameMode _gameModeEditor;
	private static GameObject _singleton;

	public static GameObject GetGlobalSingleton(){
		if(!_singleton){
			_singleton = GameObject.Find("Empty_Singleton");
		}
		return _singleton;
	}

	public static CubeGridEditorGameMode GetEditorComponent(){

		if (!_gameModeEditor){
			if(!_singleton)	GetGlobalSingleton();
			_gameModeEditor = _singleton.GetComponent<CubeGridEditorGameMode>();
		}

		return _gameModeEditor;

	}

	public static void SwitchMode(){

		if(isPlayMode){
			SetEditMode();
		}else if(isEditMode) SetPlayMode();

	}

	public static void SetEditMode(){
		GameMode = GameEnums.GameModes.EditMode;
		GetEditorComponent();
		_gameModeEditor.enabled = true;
	}

	public static void SetPlayMode(){
		GameMode = GameEnums.GameModes.PlayMode;
		GetEditorComponent();
		_gameModeEditor.enabled = false;
	}

	public static bool isPlayMode{
		get{return GameMode == GameEnums.GameModes.PlayMode;}
	}

	public static bool isEditMode{
		get{return GameMode == GameEnums.GameModes.EditMode;}
	}

	public static GameObject Player{
		get{
			if(!_Player){
				_Player = GameObject.Find("Player");
			}

			return _Player;
		}
	}

	public static GameObject CurrentBox{
		get{
			return _CurrentBox;
		}

		set{
			_CurrentBox = value;
		}
	}

	public static CubeGrid Grid{
		get{
			if(!_Grid){

                GetGlobalSingleton();				
				_Grid = _singleton.GetComponent<CubeGridSingletonObject>().Grid;

			}

			return _Grid;
		}
	}

	public static DeferredExecution DeferredExecutionComponent{
		get{
			if(!_DeferredExecutionComponent){
				GameObject EmptySingleton = GameObject.Find("Empty_Singleton");
				
				_DeferredExecutionComponent = EmptySingleton.GetComponent<DeferredExecution>();

				if(!_DeferredExecutionComponent){
					_DeferredExecutionComponent = EmptySingleton.AddComponent<DeferredExecution>();
				}
			}

			return _DeferredExecutionComponent;
		}
	}

    public static void Refresh()
    {
        _Grid = null;
    }

	public class MouseButtonsEventArgs:EventArgs{
		public KeyCode Button{get;set;}
		public MouseButtonsEventArgs(int button){
			switch (button){
			case 0: {Button = KeyCode.Mouse0; break;}
			case 1: {Button = KeyCode.Mouse1; break;}
			case 2: {Button = KeyCode.Mouse2; break;}
			default: {Button = KeyCode.Mouse0; break;}
			}
		}
	}
}
