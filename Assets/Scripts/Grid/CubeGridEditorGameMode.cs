using UnityEngine;
using System;
using System.Collections;

public class CubeGridEditorGameMode : MonoBehaviour {

	private enum EditorModes{Add,Delete,Move,Connect};

	public GameObject _markerPrefab;

	private CubeGrid _grid;
	private GameObject _marker;
	private Vector3 _markerPosition;
	private GameObject _currentObject;
	private CubeLibrary _library;
	private float _gridSize;
	private EditorModes _editorMode;
	private bool _traceMouse = true;


	public void SetActiveCube(){

	}
	public void SetAddMode() {_editorMode = EditorModes.Add;}
	public void SetDeleteMode() {_editorMode = EditorModes.Delete;}
	public void SetMoveMode() {_editorMode = EditorModes.Move;}
	public void SetConnectMode() {_editorMode = EditorModes.Connect;}

	public CubeLibrary Library{
		get {return _library;}
	}

	public int BrushObjectIndex{
		set{_grid.SelectedPrefabIndex = value;}
		get{return _grid.SelectedPrefabIndex;}
	}

	public GameObject CurrentObject{
		get {return _currentObject;}
		set {
			if (value){
				_currentObject = value;
			}
		}
	}

	public bool TraceMouse{
		set{_traceMouse = value;}
		get{return _traceMouse;}
	}

	void Start () {

		_grid     = GlobalOptions.Grid;
		_library  = _grid.m_CubeLibrary;
		_gridSize = _grid.gridSize;



	}

	void OnEnable(){

		Start();

		if (!_marker){
			// Создаем маркер
			_markerPrefab.transform.localScale = new Vector3(_gridSize, _gridSize, _gridSize);			
			_marker = Instantiate(_markerPrefab, Vector3.zero, Quaternion.Euler(0, 0, 0)) as GameObject;
			_marker.name = "MARKER";
			DestroyImmediate(_marker.GetComponent<Collider>());

		}

		// Events
		Messenger.AddListener("LeftMouseUp", OnMouseUp);

	}

	void OnDisable(){

		// Events
		Messenger.RemoveListener("LeftMouseUp", OnMouseUp);
		DestroyImmediate(_marker);

	}

	void Update () {
	
		if(_traceMouse){
			_markerPosition = GetMarkerPosition();
			_marker.transform.position = _markerPosition;
		}

	}

	Vector3 GetMarkerPosition(){

		Vector3 intersectionPoint = Vector3.zero;
		RaycastHit hitInfo = new RaycastHit();
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		
		if (Physics.Raycast(ray, out hitInfo)){
			
			if(_editorMode == EditorModes.Add){
				
				Vector3 pointOnCollider = hitInfo.transform.position + hitInfo.normal * _gridSize;
				intersectionPoint = pointOnCollider;										
				
			}else{
				
				_currentObject = hitInfo.transform.gameObject;
				intersectionPoint = hitInfo.transform.position;
				
			}
			
			intersectionPoint.x = intersectionPoint.x - intersectionPoint.x % _gridSize;
			intersectionPoint.y = intersectionPoint.y - intersectionPoint.y % _gridSize;
			intersectionPoint.z = intersectionPoint.z - intersectionPoint.z % _gridSize;
			
		}else{
			
			Plane hPlane = new Plane(Vector3.up, Vector3.zero);
			float distance = 0; 
			
			if (hPlane.Raycast(ray, out distance)){

				Vector3 pointOnXY = ray.GetPoint(distance);																			
				
				intersectionPoint  = new Vector3(pointOnXY.x - _gridSize/2, pointOnXY.y - _gridSize/2, pointOnXY.z - _gridSize/2);
				
				intersectionPoint.x = intersectionPoint.x - intersectionPoint.x % _gridSize ;
				intersectionPoint.y = intersectionPoint.y - intersectionPoint.y % _gridSize ;
				intersectionPoint.z = intersectionPoint.z - intersectionPoint.z % _gridSize ;

				GameObject debug_sphere = GameObject.Find("debug_Sphere");
				debug_sphere.transform.position = pointOnXY;

				GameObject debug_sphere2 = GameObject.Find("debug_Sphere2");
				debug_sphere2.transform.position = intersectionPoint;

				if (Physics.Raycast(pointOnXY, intersectionPoint, out hitInfo, Vector3.Distance(pointOnXY, intersectionPoint))){
				
				
					intersectionPoint = hitInfo.point;

					intersectionPoint.x = intersectionPoint.x - intersectionPoint.x % _gridSize;
					intersectionPoint.y = intersectionPoint.y - intersectionPoint.y % _gridSize;
					intersectionPoint.z = intersectionPoint.z - intersectionPoint.z % _gridSize;									   

				}
			}
		}
		

		return intersectionPoint;

	} 

	private void OnMouseUp(object sender, EventArgs evArgs){

		if(!_traceMouse){return;};

		switch (_editorMode){
			case EditorModes.Add:{
				GameObject lastCube;
				_grid.CreateCubeAt(_markerPosition, out lastCube);
				break;
			}

		}

	}

}
