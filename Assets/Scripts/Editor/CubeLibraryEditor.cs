using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

[CustomEditor(typeof(CubeLibrary))]
public class CubeLibraryEditor : Editor {
	
	public override void OnInspectorGUI(){		

		CubeLibrary TargetLibrary = (CubeLibrary)target;
		List<Cube> CubeList = TargetLibrary.GetList();

		DrawDefaultInspector();
		
		if (GUILayout.Button("Add")){
			//Cube obj = ScriptableObject.CreateInstance<Cube>();
			Cube obj = new Cube();
			CubeList.Add(obj);
		}
			
		if (GUILayout.Button("Clear")){
			CubeList.Clear();
		}

	}

}
