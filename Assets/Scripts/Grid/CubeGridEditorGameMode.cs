using UnityEngine;
using System;
using System.Collections;

public class CubeGridEditorGameMode : MonoBehaviour {

	private enum EditorModes{Add,Delete,Move,Connect,Target};

	public GameObject _markerPrefab;

	private CubeGrid _grid;
	private GameObject _marker;
	private Vector3 _markerPosition;
	private GameObject _currentObject;
	private CubeLibrary _library;
	private float _gridSize;
	private EditorModes _editorMode;
	private EditorModes _previousEditorMode;
	private bool _traceMouse = true;
	private GameObject _selectedObject;


	private EditorModes EditorMode{
		set{_previousEditorMode = _editorMode; _editorMode = value;}
		get{return _editorMode;}
	}

	private EditorModes PreviousEditorMode{
		get{return _previousEditorMode;}
	}

	public void SetActiveCube(){

	}
	public void SetAddMode() {EditorMode = EditorModes.Add;}
	public void SetDeleteMode() {EditorMode = EditorModes.Delete;}
	public void SetMoveMode() {EditorMode = EditorModes.Move;}
	public void SetConnectMode() {EditorMode = EditorModes.Connect;}

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
		Messenger.AddListener("LeftMouseDown", OnMouseDown);

	}

	void OnDisable(){

		// Events
		Messenger.RemoveListener("LeftMouseUp", OnMouseUp);
		Messenger.RemoveListener("LeftMouseDown", OnMouseDown);
		DestroyImmediate(_marker);

	}

	void Update () {
	
		if(_traceMouse){
			_markerPosition = GetMarkerPosition();
			_marker.transform.position = _markerPosition;
		}

		ProcessTargetMode();

	}

	private void ProcessTargetMode(){
		if (EditorMode != EditorModes.Target
		    || !_selectedObject) return;



	}

	private void DrawParabola(){

		int Steps = 5;

		for(int i = 1; i<=Steps; i++)

	}

	private Vector3 SampleParabola( Vector3 start, Vector3 end, float height, float t ) {
		float parabolicT = t * 2 - 1;
		if ( Mathf.Abs( start.y - end.y ) < 0.1f ) {
			//start and end are roughly level, pretend they are - simpler solution with less steps
			Vector3 travelDirection = end - start;
			Vector3 result = start + t * travelDirection;
			result.y += ( -parabolicT * parabolicT + 1 ) * height;
			return result;
		} else {
			//start and end are not level, gets more complicated
			Vector3 travelDirection = end - start;
			Vector3 levelDirecteion = end - new Vector3( start.x, end.y, start.z );
			Vector3 right = Vector3.Cross( travelDirection, levelDirecteion );
			Vector3 up = Vector3.Cross( right, travelDirection );
			if ( end.y > start.y ) up = -up;
			Vector3 result = start + t * travelDirection;
			result += ( ( -parabolicT * parabolicT + 1 ) * height ) * up.normalized;
			return result;
		}
	}

	Vector3 GetMarkerPosition(){

		Vector3 intersectionPoint = Vector3.zero;
		RaycastHit hitInfo = new RaycastHit();
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		
		if (Physics.Raycast(ray, out hitInfo)){

			switch (EditorMode){
				case EditorModes.Add:{
					
						Vector3 pointOnCollider = hitInfo.transform.position + hitInfo.normal * _gridSize;
						intersectionPoint = pointOnCollider;										
						break;
					}	

				case EditorModes.Delete: 
				case EditorModes.Connect:
				case EditorModes.Move:
					{				
						_currentObject = hitInfo.transform.gameObject;
						intersectionPoint = hitInfo.transform.position;
						break;
					}
				
			}
			
			intersectionPoint.x = Mathf.Round(intersectionPoint.x / _gridSize) * _gridSize;
			intersectionPoint.y = Mathf.Round(intersectionPoint.y / _gridSize) * _gridSize;
			intersectionPoint.z = Mathf.Round(intersectionPoint.z / _gridSize) * _gridSize;
			
		}else{
			
			Plane hPlane = new Plane(Vector3.up, Vector3.zero);
			float distance = 0; 
			
			if (hPlane.Raycast(ray, out distance)){

				Vector3 pointOnXY = ray.GetPoint(distance);																			
				
				intersectionPoint  = new Vector3(pointOnXY.x, pointOnXY.y, pointOnXY.z);
				
				intersectionPoint.x = Mathf.Round(intersectionPoint.x / _gridSize) * _gridSize;
				intersectionPoint.y = Mathf.Round(intersectionPoint.y / _gridSize) * _gridSize;
				intersectionPoint.z = Mathf.Round(intersectionPoint.z / _gridSize) * _gridSize;

				}
		}
		

		return intersectionPoint;

	} 

	private void OnMouseUp(object sender, EventArgs evArgs){

		if(!_traceMouse){return;};

		switch (EditorMode){
			case EditorModes.Add:{
				GameObject lastCube;
				_grid.CreateCubeAt(_markerPosition, out lastCube);
				break;
			}
			case EditorModes.Delete:{
				_grid.DeleteCubeAt(_markerPosition);
				break;
			}

		}

	}

	private void OnMouseDown(object sender, EventArgs evArgs){
			
			if(!_traceMouse){return;};
			
			switch (EditorMode){
			case EditorModes.Move:{
				_selectedObject = _grid.GetCubeAt(_markerPosition);
				EditorMode = EditorModes.Target;
				_marker.gameObject.SetActive(false);
				break;
			}
			case EditorModes.Connect:{
				_selectedObject = _grid.GetCubeAt(_markerPosition);
				EditorMode = EditorModes.Target;
				break;
			}
				
			}


	}

}
