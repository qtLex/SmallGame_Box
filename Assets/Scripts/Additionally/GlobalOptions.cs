using UnityEngine;
using System.Collections;

public static class GlobalOptions
{

	private static GameObject _Player;
	private static GameObject _CurrentBox;
	private static CubeGrid   _Grid;
	private static DeferredExecution _DeferredExecutionComponent;

	// + mors
	private static GameEnums.GameModes GameMode;
	private static CubeGridEditorGameMode _gameModeEditor;
	private static GameObject _singleton;

	public static GameObject GetGlobalSingleton(){
		if(!_singleton){
			_singleton = GameObject.Find("Empty_Singleton");
		}
		return _singleton;
	}

	private static void GetEditorComponent(){

		if (!_gameModeEditor){
			if(!_singleton)	GetGlobalSingleton();
			_gameModeEditor = _singleton.GetComponent<CubeGridEditorGameMode>();
		}
		
	}

	public static void SwitchMode(){

		if(isPlayMode) SetEditMode();
		if(isEditMode) SetPlayMode();

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

	// -mors

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
}
