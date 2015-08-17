using UnityEngine;
using System.Collections;

public class CubeGridEditorGameMode : MonoBehaviour {

	private enum EditorModes{Add,Delete};

	public GameObject _markerPrefab;

	private CubeGrid _grid;
	private GameObject _marker;
	private GameObject _currentObject;
	private CubeLibrary _library;
	private float _gridSize;
	private EditorModes _editorMode;
	private bool _traceMouse = true;

	public CubeLibrary Library{
		get {return _library;}
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

	}

	void OnDisable(){

		DestroyImmediate(_marker);

	}
	
	// Update is called once per frame
	void Update () {
	
		if(_traceMouse){
			_marker.transform.position = GetMarkerPosition();
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
				
				intersectionPoint  = new Vector3(pointOnXY.x, pointOnXY.y, pointOnXY.z);
				
				intersectionPoint.x = intersectionPoint.x - intersectionPoint.x % _gridSize;
				intersectionPoint.y = intersectionPoint.y - intersectionPoint.y % _gridSize;
				intersectionPoint.z = intersectionPoint.z - intersectionPoint.z % _gridSize;
				
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

}
