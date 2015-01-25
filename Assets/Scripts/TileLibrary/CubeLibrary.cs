using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

[ExecuteInEditMode]
[System.Serializable]
public class CubeLibrary : ScriptableObject {
	[SerializeField]
	private List<Cube> CubeList;

	void Start(){
		if (CubeList == null){CubeList = new List<Cube>(); Debug.Log("Creating library list...");};
	}

	public Texture[] GetImageList(){

		List<Texture2D> texlist = new List<Texture2D>();
		foreach(Cube iterator in CubeList){
			if(iterator.Object != null){
				texlist.Add(AssetPreview.GetAssetPreview((iterator.Object)));
			}
		}

		return (Texture[])texlist.ToArray();
	}

	public KeyValuePair<string, GameObject>[] GetObjectsAndGuids(){

	   	KeyValuePair<string, GameObject>[] result = new KeyValuePair<string, GameObject>[CubeList.Count];
		int i = 0;
		foreach(Cube iterator in CubeList){
			result[i++] = new KeyValuePair<string, GameObject>(iterator.Key, iterator.Object);
		}
		
		return result;

	}

	public GameObject[] GetObjects(){

		List<GameObject> objList = new List<GameObject>();
		foreach(Cube iterator in CubeList){
			if(iterator.Object != null){
				objList.Add(iterator.Object);
			}
		}
		
		return objList.ToArray();
	
	}

	public GameObject GetGameObjectByIndex(int index){

		if ((CubeList != null) && CubeList.Count > 0){
			return CubeList[index].Object;
		}

		return null;

	}

	public List<Cube> GetList(){
		return CubeList;
	}

	public string GetGUIDByObject(GameObject _object){

		CubeTagBehavior TagComponent = _object.GetComponent<CubeTagBehavior>();

		if(TagComponent != null){
			return TagComponent.guid;
		}

		return "";
	}
}
