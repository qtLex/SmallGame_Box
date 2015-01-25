using UnityEngine;
using System.Collections;

public static class GlobalOptions
{
	private static GameObject _Player;
	private static GameObject _CurrentBox;
	private static CubeGrid   _Grid;
	private static DeferredExecution _DeferredExecutionComponent;

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
				GameObject EmptySingleton = GameObject.Find("Empty_Singleton");
				
				_Grid = EmptySingleton.GetComponent<CubeGridSingletonObject>().Grid;
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
