using UnityEngine;
using System.Collections;
using System.IO;
using UnityEditor;

[CustomEditor(typeof(LevelManager))]
public class LevelManagertEditor : Editor 
{
	int LevelIndex = 0;
	public override void OnInspectorGUI()
	{
		LevelManager myTarget = (LevelManager)target;

		DrawDefaultInspector();

		GUILayout.Label(myTarget.LevelFolder);
		if(GUILayout.Button("Folder")){

			string NewFolder = EditorUtility.OpenFolderPanel("LevelsFolder", myTarget.LevelFolder,"");
			if (NewFolder.Length != 0){
				myTarget.LevelFolder = NewFolder;
			}
		}
		
		if(GUILayout.Button("Reload")){
			myTarget.RefreshMe();
		}


		GUILayout.BeginHorizontal();
		if(GUILayout.Button("Load by index")){
			myTarget.LoadByIndex(LevelIndex);
		}

		if(myTarget.LevelList.Count > 0){
			LevelIndex = EditorGUILayout.IntField(LevelIndex);
			LevelIndex = Mathf.Min(LevelIndex, myTarget.LevelList.Count);
		}
		GUILayout.EndHorizontal();
	}
}

