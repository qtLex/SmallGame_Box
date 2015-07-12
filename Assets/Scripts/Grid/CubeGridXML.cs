using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

// Сериализатор сетки в файл XML. Обёртка для класса CubeGrid.
// Возможно чтение из файла с созданием новой сетки.
[XmlRoot("CubeGridXML")]
public class CubeGridXML{

	[XmlAttribute("Library")]
	public string CubeLibraryPath;

	[XmlAttribute("Size")]
	public float CubeSize;
	
	[XmlArray("Values")]
	[XmlArrayItem("Cube")]

	public SerializableXMLElement[] Elements;

	public struct StructNeedTarget
	{
		public GameObject obj;
		public Vector3 targetPos;
		public iUseTarget comp;

		public void SetArgs(GameObject Obj, Vector3 target)
		{
			obj = Obj;
			targetPos = target;
			comp = Obj.GetComponent<iUseTarget>();
		}
	}

	[Serializable]
	public struct Vector3Serializer
	{
		[XmlAttribute("x")]
		public float x;
		[XmlAttribute("y")]
		public float y;
		[XmlAttribute("z")]
		public float z;
		
		public void Fill(Vector3 v3)
		{
			x = v3.x;
			y = v3.y;
			z = v3.z;
		}
		
		public Vector3 V3
		{ get { return new Vector3(x, y, z); } }
	}

	public class SerializableXMLElement{
		[XmlAttribute("key")]
		public string _key;
		[XmlAttribute("index")]
		public string _guid;
		[XmlAttribute("usetarget")]
		public bool _usetarget;

        [XmlElement(typeof(Vector3Serializer))]
        public Vector3Serializer _position;
		[XmlElement(typeof(Vector3Serializer))]
		public Vector3Serializer _target;
        [XmlElement(typeof(Vector3Serializer))]
        public Vector3Serializer _forward;
        [XmlElement(typeof(Vector3Serializer))]
        public Vector3Serializer _up;
        [XmlElement(typeof(Vector3Serializer))]
        public Vector3Serializer _right;
		
		public SerializableXMLElement(){}
		
		public SerializableXMLElement(string key, string guid, Vector3 pos, Vector3 forward, Vector3 up, Vector3 right){
			
			_key = key;
			_guid = guid;
			_usetarget = false;

			_position = new Vector3Serializer();
			_position.Fill(pos);
            
       		_forward = new Vector3Serializer();
			_forward.Fill(forward);
            
			_up = new Vector3Serializer();
			_up.Fill(up);

			_right = new Vector3Serializer();
			_right.Fill(right);
			
		}

		public SerializableXMLElement(string key, string guid, Vector3 pos, Vector3 targ, Vector3 forward, Vector3 up, Vector3 right){

			_key = key;
			_guid = guid;
			_position = new Vector3Serializer();
			_position.Fill(pos);

			_usetarget = true;

			_target = new Vector3Serializer();
			_target.Fill(targ);

			_forward = new Vector3Serializer();
			_forward.Fill(forward);
			
			_up = new Vector3Serializer();
			_up.Fill(up);
			
			_right = new Vector3Serializer();
			_right.Fill(right);
		}
		
	}

	public CubeGridXML(){

	}

	public static CubeGridXML LoadFromFile(string path){

		XmlSerializer serializer = new XmlSerializer(typeof(CubeGridXML));
		using(Stream stream = new FileStream(path, FileMode.Open))
		{
			return serializer.Deserialize(stream) as CubeGridXML;
		}

	}

	public CubeGridXML(CubeGrid original, string path){

		Elements = original.GetElements();
		CubeSize =  original.gridSize;
		CubeLibraryPath = AssetDatabase.GetAssetPath(original.m_CubeLibrary);

		XmlSerializer serializer = new XmlSerializer(typeof(CubeGridXML));
		using(Stream stream = new FileStream(path, FileMode.OpenOrCreate))
		{
			serializer.Serialize(stream, this);
		}

	}

	public static bool CanBeDeserialized(string path){

		CubeGridXML XMLObject =	LoadFromFile(path);

		return (XMLObject != null);

	}

	public static CubeGrid ToGrid(string path){

		CubeGridXML XMLObject =	LoadFromFile(path);
		CubeGrid _grid = ScriptableObject.CreateInstance<CubeGrid>();
		_grid.m_CubeLibrary = AssetDatabase.LoadAssetAtPath(XMLObject.CubeLibraryPath, typeof(CubeLibrary)) as CubeLibrary;
		_grid.gridSize = XMLObject.CubeSize;

		CubeLibrary _lib = _grid.m_CubeLibrary;
		KeyValuePair<string, GameObject>[] LibraryList = _lib.GetObjectsAndGuids();
		List<StructNeedTarget> NeedTarget = new List<StructNeedTarget>();


		foreach(SerializableXMLElement iterator in XMLObject.Elements){
			GameObject NewCube = null;
			string NewGuid = "";
			int index = 0;
			foreach(KeyValuePair<string, GameObject> _pairiterator in LibraryList){
				if (_pairiterator.Key == iterator._guid){
					NewCube = _lib.GetGameObjectByIndex(index);
					NewGuid = iterator._guid;
					break;
				}
				index++;
			}
			
			GameObject CreatedCube = null;
			if (NewCube == null){ continue;}

			_grid.currentPrefab = NewCube;
			_grid.currentPrefabGuid = NewGuid;
			_grid.CreateCubeAt(iterator._position.V3, out CreatedCube);

			CreatedCube.transform.forward = iterator._forward.V3;
			CreatedCube.transform.up      = iterator._up.V3;
			CreatedCube.transform.right   = iterator._right.V3;

			if(iterator._usetarget)
			{
				StructNeedTarget CurrentTarget = new StructNeedTarget();
				CurrentTarget.SetArgs(CreatedCube, iterator._target.V3);
				NeedTarget.Add(CurrentTarget);
			}

		}

		// пройдемся и проставим таргет по позиции
		foreach(StructNeedTarget targetObj in NeedTarget.ToArray())
		{
			targetObj.comp.SetTarget(_grid.GetCubeAt(targetObj.targetPos));
		}


		return _grid;

	}

}
