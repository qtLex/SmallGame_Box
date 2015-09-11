using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;
using EventArgsChildren;

public class CubeGridEditorGameMode : MonoBehaviour {

	private enum EditorModes{Add,Delete,Move,Connect,Target};

	public GameObject MarkerPrefab;
	public GameObject TargetLine;
	public Text ToolTipText;
	public int TargetLineSteps = 10;

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
	private GameObject _targetLine;
	private int _targetLineSteps = 0;


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
			MarkerPrefab.transform.localScale = new Vector3(_gridSize, _gridSize, _gridSize);			
			_marker = Instantiate(MarkerPrefab, Vector3.zero, Quaternion.Euler(0, 0, 0)) as GameObject;
			_marker.name = "MARKER";
			DestroyImmediate(_marker.GetComponent<Collider>());

		}

		// Events
		Messenger.AddListener("MouseUp", OnMouseUp);
		Messenger.AddListener("MouseDown", OnMouseDown);
		
	}

	void OnDisable(){

		// Events
		Messenger.RemoveListener("MouseUp", OnMouseUp);
		Messenger.RemoveListener("MouseDown", OnMouseDown);

		DestroyImmediate(_marker);
		DestroyImmediate(_targetLine);

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
		    || !_selectedObject){
			if (_targetLine){
				GameObject.DestroyImmediate(_targetLine);
			}
			ToolTipText.gameObject.SetActive(false);
			return;
		}

		if(ToolTipText){
			ToolTipText.gameObject.SetActive(true);
			ToolTipText.transform.position = Input.mousePosition;
			ToolTipText.text = PreviousEditorMode.ToString();
		}

		DrawParabola();

	}

	private void ProcessTargetClick(){

		switch(PreviousEditorMode){
			case EditorModes.Move:{
				if (!_selectedObject) break;
				_grid.MoveCube(_selectedObject.transform.position, _markerPosition);
				break;
			}
			case EditorModes.Connect:{
				BoxClasses.BaseBox component = _selectedObject.GetComponentInChildren<BoxClasses.BaseBox>();
				if (component.CanBeConnected()){
					GameObject targetObject = _grid.GetCubeAt(_markerPosition);
					component.ConnectTo(targetObject);
				}
				break;
			}

		}

		EditorMode = PreviousEditorMode;

	}

	private void DrawParabola(){
		
		if (!_targetLine){
			_targetLine = GameObject.Instantiate<GameObject>(TargetLine);
		}

		LineRenderer lineRenderer = _targetLine.GetComponent<LineRenderer>();
		//if (_targetLineSteps != TargetLineSteps + 1){
			_targetLineSteps = TargetLineSteps + 1;
			lineRenderer.SetVertexCount(_targetLineSteps + 1);
			lineRenderer.SetWidth(_gridSize/5.0f, _gridSize / 10.0f);
		//}

		for(int i = 0; i <=_targetLineSteps; i++){
			Vector3 lineVertex = SampleParabola(_selectedObject.transform.position, _markerPosition, _gridSize * 2.0f, (float)i / _targetLineSteps);
			lineRenderer.SetPosition(i, lineVertex);
		}

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
			Vector3 levelDirection = end - new Vector3( start.x, end.y, start.z );
			Vector3 right = Vector3.Cross( travelDirection, levelDirection );
			Vector3 up = Vector3.Cross(right, levelDirection);
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
				case EditorModes.Target:{

					switch(PreviousEditorMode){
						case EditorModes.Move:{
							Vector3 pointOnCollider = hitInfo.transform.position + hitInfo.normal * _gridSize;
							intersectionPoint = pointOnCollider;										
							break;
						}
						case EditorModes.Connect:{
							_currentObject = hitInfo.transform.gameObject;
							intersectionPoint = hitInfo.transform.position;
							break;
						}
					}
				break;
				}
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

        if (!_traceMouse)
            return;

        if (evArgs != null)
        {
            MouseButtonsEventArgs eventArguments = (MouseButtonsEventArgs)evArgs;// Не красиво! А если указатель не на тип MouseButtonEventArgs? Каст отработает, но данные будут не валидные.

            if (eventArguments.Button == KeyCode.Mouse0)
            {
                switch (EditorMode)
                {
                    case EditorModes.Add:
                        {
                            GameObject lastCube;
                            _grid.CreateCubeAt(_markerPosition, out lastCube);
                            break;
                        }
                    case EditorModes.Delete:
                        {
                            _grid.DeleteCubeAt(_markerPosition);
                            break;
                        }
                    case EditorModes.Move:
                    case EditorModes.Connect:
                        {
                            _selectedObject = _grid.GetCubeAt(_markerPosition);
                            if (_selectedObject)
                            {
                                EditorMode = EditorModes.Target;
                            }
                            break;
                        }
                    case EditorModes.Target:
                        {
                            ProcessTargetClick();
                            break;
                        }
                }

            }
            else if (eventArguments.Button == KeyCode.Mouse1)
            {

                switch (EditorMode)
                {
                    case EditorModes.Target:
                        {
                            EditorMode = PreviousEditorMode;
                            break;
                        }
                }
            }
        }

    }

	private void OnMouseDown(object sender, EventArgs evArgs)
    {
        if (!_traceMouse)
            return;

        if (evArgs != null)
        {
             MouseButtonsEventArgs eventArguments = (MouseButtonsEventArgs)evArgs;// Не красиво! А если указатель не на тип MouseButtonEventArgs? Каст отработает, но данные будут не валидные.
        }
	}

}
