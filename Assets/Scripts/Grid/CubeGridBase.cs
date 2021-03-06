﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CubeGridBase : ScriptableObject
{

  public GameObject currentPrefab;
  public string currentPrefabGuid;
  private int currentPrefabIndex;
  public CubeLibrary m_CubeLibrary;
  public float gridSize = 10.0f;
  private GameObject Parent;
  public Dictionary<string, GameObject> Elements = new Dictionary<string, GameObject>();

  public int SelectedPrefabIndex{
		set{
			GameObject go = m_CubeLibrary.GetGameObjectByIndex(value);
			if(go){
				currentPrefab = go;
				currentPrefabGuid = m_CubeLibrary.GetGUIDByObject(currentPrefab);
				currentPrefabIndex = value;
			}

		}
		get{
			return currentPrefabIndex;
		}
  }

  private Vector3 decimateCoords(Vector3 coords, out string key)
  {
        
    Vector3 result = new Vector3();
        
    result.x = coords.x - coords.x % gridSize;
    result.y = coords.y - coords.y % gridSize;
    result.z = coords.z - coords.z % gridSize;
        
    key = result.ToString();
        
    return  result;
        
  }

  public bool CreateCubeAt(Vector3 coords){

		GameObject temp;
		bool result = CreateCubeAt(coords, out temp);
		temp = null;
		return result;
  }

  public bool CreateCubeAt(Vector3 coords, out GameObject cube)
  {
        
    string newKey;
    Vector3 normCoords = decimateCoords(coords, out newKey);
    GameObject existingCube;
        
    if ((Elements.TryGetValue(newKey, out existingCube)) && existingCube != null)
      {
            
        cube = existingCube;
        Debug.Log("Cube already exists at coordinate " + newKey);
            
        return false;
            
      } else
        {
            
          Debug.Log("Creating new cube at " + newKey);

          GameObject Parent = GameObject.Find("GridRoot");
          if (Parent == null)
            {
              Parent = new GameObject("GridRoot");
              Parent.hideFlags = HideFlags.NotEditable | HideFlags.HideInInspector;
            }

            GameObject blockInst = currentPrefab;
            blockInst.transform.localScale = new Vector3(gridSize, gridSize, gridSize);     
            GameObject objInstance = Instantiate(blockInst, normCoords, new Quaternion(0, 0, 0, 1)) as GameObject;
            objInstance.transform.parent = Parent.transform;
            CubeTagBehavior TagComponent = objInstance.AddComponent<CubeTagBehavior>();
            TagComponent.guid = currentPrefabGuid;

            if (objInstance.GetComponent<iUseTarget>() == null)
              objInstance.hideFlags = HideFlags.NotEditable;

            Elements.Add(newKey, objInstance);
            
            cube = objInstance;
            return true;
            
        }
        
  }
    
  	public bool DeleteCubeAt(Vector3 coords)
  	{
        
    string newKey = "";
    decimateCoords(coords, out newKey);
    GameObject existingCube;

    if (Elements.TryGetValue(newKey, out existingCube))
      {
      
        DestroyImmediate(existingCube);
        Elements.Remove(newKey);
      
        return true;
      
      } else
        {
          return false;
        }
  	}

	// Перемещает куб с одной из позиций или свапает кубы в обеих позициях
	//
	public bool MoveCube(Vector3 from, Vector3 to){
		
		string key_from, key_to;
		GameObject first,second;
		Vector3 dec_from = decimateCoords(from, out key_from);
		Vector3 dec_to   = decimateCoords(to, out key_to);

		if(!Elements.TryGetValue(key_from, out first) &&
		   !Elements.TryGetValue(key_to, out second))
		{
            return false;
        }
	
		if(first && !second){

			Elements.Remove(key_from);
			Elements.Add(key_to, first);
			first.transform.position = dec_to;

		}else if (!first && second){

			Elements.Remove(key_to);
			Elements.Add(key_from, second);
			second.transform.position = dec_from;

		}else if(first && second){

			Elements.Remove(key_from);
			Elements.Remove(key_to);

			Elements.Add(key_from, second);
			Elements.Add(key_to, first);

			second.transform.position = dec_from;
			first.transform.position = dec_to;
		
		}
		
		return true;
	}
  
   	public GameObject GetCubeAt(Vector3 coords)
   	{
		string key = "";
		decimateCoords(coords, out key);
		GameObject existingCube;

		Elements.TryGetValue(key, out existingCube);

		return existingCube;
   	}

   	public void ClearDictionary()
   	{
      foreach (GameObject _gameObject in Elements.Values)
      {

          DestroyImmediate(_gameObject);

      }

      Elements.Clear();
  }
}
