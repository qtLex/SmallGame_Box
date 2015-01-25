using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;
using System.Xml;
using System.Xml.Serialization;

public class CubeGrid : CubeGridBase {

	[Serializable]
	public class SerializableGridElement{

		public string _key;
		public GameObject _object;

		public SerializableGridElement(string key, GameObject gameobject){

			_key = key;
			_object = gameobject;

		}
	};

	[SerializeField]
	private SerializableGridElement[] _internalArray;

	#region События редактора

	void OnEnable(){
		ConvertToDictionary();
	}

	#endregion

	#region Сериализация между сеансами

	public CubeGrid(){
		EditorApplication.playmodeStateChanged += ModeChangeCallback;
	}

	public void ModeChangeCallback(){

		if (EditorApplication.isPlayingOrWillChangePlaymode && !EditorApplication.isPlaying)
		{
			ConvertToList();
		}

	}

	private void ConvertToList(){

		if(base.Elements.Count == 0) return;

		Debug.Log("Converting to list. " + Elements.Count.ToString() + " elements converted.");

		List<SerializableGridElement>  _internalList = new List<SerializableGridElement>();

		foreach(KeyValuePair<string, GameObject> iterator in Elements){

			_internalList.Add(new SerializableGridElement(iterator.Key, iterator.Value));
		}

		_internalArray = _internalList.ToArray();
	}

	private void ConvertToDictionary(){

		if(_internalArray == null) return;

		Debug.Log("Trying to convert back to dictionary. Serialized list: " + _internalArray.ToString());

		if (_internalArray.Length == 0) return;

		ClearDictionary();

		Debug.Log("Converting to dictionary. " + _internalArray.Length + " elements converted");

		List<SerializableGridElement>  _internalList = new List<SerializableGridElement>();

		foreach(SerializableGridElement iterator in _internalArray){

			if(iterator._object == null){
				continue;
			}

			_internalList.Add(iterator);
			Elements.Add(iterator._key, iterator._object);

		}

		_internalArray = _internalList.ToArray();

	
	}
	#endregion

	#region Сериализация объекта
	
	public void SerializeMe(string path){
		
		new CubeGridXML(this, path);
		
	}

	public CubeGridXML.SerializableXMLElement[] GetElements(){

		ConvertToList();
		CubeGridXML.SerializableXMLElement[] _XMLArray = new CubeGridXML.SerializableXMLElement[_internalArray.Length]; 
		int i = 0;
		foreach(SerializableGridElement iterator in _internalArray){
			if (iterator._object != null){

				_XMLArray[i++] = new CubeGridXML.SerializableXMLElement(iterator._key,
						 m_CubeLibrary.GetGUIDByObject(iterator._object),
				                     iterator._object.transform.position);
			
			}
		}

		return _XMLArray;
	}
	
	#endregion

}


