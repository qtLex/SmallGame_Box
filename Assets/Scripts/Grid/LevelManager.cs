using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System;

// Выбирать нужно тоьлко каталоги верхнего уровня в каталоге Assets.
public class LevelManager : MonoBehaviour {

	[HideInInspector]
	public string LevelFolder = "";
	private CubeGrid _grid;
	public bool UseDefaultFolder = false;

	[SerializeField]
	public List<Level> LevelList = new List<Level>();

	[Serializable]
	public class Level{
		public string path;
		public Level(string _path){
			path = _path;
		}
	}

	void Start () {
		CubeGridSingletonObject sing = FindObjectOfType<CubeGridSingletonObject>() as CubeGridSingletonObject;
		if(sing != null){
			_grid = sing.Grid;
		}

		RefreshLevelList(!UseDefaultFolder ? LevelFolder : Application.dataPath+"/Levels");

	}

	public void RefreshMe(){

		RefreshLevelList(!UseDefaultFolder ? LevelFolder : Application.dataPath+"/Levels");
	}
	
	void RefreshLevelList(string path){

		if (path.Length == 0){
			return;
		}

		LevelList.Clear();

		#if UNITY_EDITOR
		DirectoryInfo dir = new DirectoryInfo(path);
		#else
		DirectoryInfo dir = new DirectoryInfo(Path.Combine(Application.dataPath, "Levels"));
		#endif

		Debug.Log(path);

		if (!dir.Exists){
			Debug.LogError("No Level direcory has being found");
		}

		Debug.Log("Reading level data...");
		FileInfo[] info = dir.GetFiles("*.xml");
		int count = 0;
		foreach (FileInfo f in info) 
		{

			if ( CubeGridXML.CanBeDeserialized(f.FullName)){
				LevelList.Add(new Level(f.FullName));
				Debug.Log("File " + f.FullName + "recognized as a level...");
				count++;
			}
			
		}

		Debug.Log("Total: " + count + " levels loaded.");

	}

	public void LoadByIndex(int _index){

		if (_grid == null){
			CubeGridSingletonObject sing = FindObjectOfType<CubeGridSingletonObject>() as CubeGridSingletonObject;
			if(sing != null){
				_grid = sing.Grid;
			}
		}

		_grid.ClearDictionary();
		_grid = CubeGridXML.ToGrid(LevelList[_index].path);

	}

	public void LoadByPath(string _path){
		
		if (_grid == null){
			CubeGridSingletonObject sing = FindObjectOfType<CubeGridSingletonObject>() as CubeGridSingletonObject;
			if(sing != null){
				_grid = sing.Grid;
			}
		}
		
		_grid.ClearDictionary();
		_grid = CubeGridXML.ToGrid(_path);
		
	}

}
