using UnityEngine;
using UnityEditor;
using System.IO;

[ExecuteInEditMode]
[CustomEditor(typeof(CubeGrid))]

// Класс отвечает за отрисовку интерфейса объекта сетки.
// В сцене всегда присутствует одна сетка, с ней и работает класс.
// Для выделения/создания сетки нузжно выбрать пункт меню Grids -> Edit grid.
public class CubeGridEditor : Editor {
	
	private Vector3 intersectionPoint;
	private static EditMode m_editMode;
	private static CubeGrid m_Instance;
	private GameObject m_currentObject;
	private GameObject m_marker;
	private Vector3 pointOnXY;
	private Material m_markerMaterial;
	private MeshRenderer meshRenderer;

	[SerializeField]
	public static CubeLibrary m_Library;
	public static string m_LibraryPath = "";
	private int selectedCubeIndex = 0;
	private Vector2 scrollPosCubeGrid = new Vector2(0,0);
	private bool ShowCubeGridInternal = false;
	private bool ShowCubeGrid = false;

	//int GridWindowID = 0;

	#region menu

	[MenuItem ("Grids/Create library")]
	static void PickCubeLibrary(){

		// Попытаемся открыть закрепленную за сеткой библиотеку.
		if (m_LibraryPath.Length == 0){
			m_LibraryPath = EditorUtility.SaveFilePanelInProject("Select library path","New library", "asset", "Select a file", m_LibraryPath);
		}

		if (m_LibraryPath.Length != 0){

				int answer = EditorUtility.DisplayDialogComplex("Error", "Cant open library asset. Create new base?", "Yes", "Cancel", "Pick another");
				switch(answer){
				case 0:{
					m_Library = ScriptableObject.CreateInstance<CubeLibrary>();
					AssetDatabase.CreateAsset(m_Library, m_LibraryPath);
					break;}
				case 2:{
					m_LibraryPath = "";
					PickCubeLibrary(); break;
				}
			}
		}

	}

	[MenuItem ("Grids/Edit grid")]
	static void CreateGrid () {

		CubeGridSingletonObject sing =  GameObject.FindObjectOfType<CubeGridSingletonObject>();
		if (sing == null){

			GameObject EmptyObject = new GameObject("Empty_Singleton");
			sing = EmptyObject.AddComponent<CubeGridSingletonObject>();
		}

		m_Instance = ScriptableObject.FindObjectOfType<CubeGrid>();
		if(!m_Instance){
			m_Instance = ScriptableObject.CreateInstance<CubeGrid>();
		}

		sing.Grid = m_Instance;
		Selection.activeObject = m_Instance;
	}

	#endregion

	public CubeGridEditor(){
		EditorApplication.playmodeStateChanged += ModeChangeCallback;
	}

	public enum EditMode{
		addBlock, delBlock
	};

	#region События окон редактора

	void OnEnable(){
		SceneView.onSceneGUIDelegate += OnSceneGUI;
	}

	void OnDisable(){
		DestroyImmediate(m_marker);
		SceneView.onSceneGUIDelegate -= OnSceneGUI;
	}

	public void ModeChangeCallback(){
		
		if (EditorApplication.isPlayingOrWillChangePlaymode && !EditorApplication.isPlaying)
		{
			DeselectThisGrid();
		}
		
	}

	void OnSceneGUI (SceneView _view){

		// нет выделенного объекта - нет инспектора.
		if (!target) return;

		// Получаем указатель на выделенныйобъект.
		m_Instance = ((CubeGrid)target);

		if (!m_Instance) return;
		if (m_Instance.m_CubeLibrary == null){return;};

		m_Library = m_Instance.m_CubeLibrary;

		if (!m_Instance.currentPrefab){
			//Попытаемся получить первый куб из библиотеки, если он не выбран.
			m_Instance.currentPrefab = m_Library.GetGameObjectByIndex(0);
			if (!m_Instance.currentPrefab){
				// Получить не удалось, в библиотеке нет элементов.
				return;
			}
		};

		if (processGUIMenus(_view)) return;

		float blockSize = m_Instance.gridSize;
		//float halfBlockSize = blockSize / 2.0f;
		
		Quaternion noRotation = Quaternion.Euler(0, 0, 0);

		if (Event.current.type == EventType.layout){

			if (!m_marker){
				// Создаем маркер	
				GameObject blockInst = Resources.LoadAssetAtPath<GameObject>("Assets/Tiles/pre_Cursor.prefab");

				blockInst.transform.localScale = new Vector3(blockSize, blockSize, blockSize);

				m_marker = Instantiate(blockInst, intersectionPoint, noRotation) as GameObject;
				m_marker.name = "MARKER";
				m_marker.hideFlags = HideFlags.HideAndDontSave | HideFlags.NotEditable | HideFlags.HideInInspector | HideFlags.HideInHierarchy;
				DestroyImmediate(m_marker.GetComponent<BoxCollider>());
				meshRenderer = m_marker.GetComponent<MeshRenderer>();
				m_markerMaterial = new Material(meshRenderer.sharedMaterial);

				m_markerMaterial.color = Color.Lerp(Color.green, meshRenderer.sharedMaterial.color, 0.85f);
				meshRenderer.sharedMaterial = m_markerMaterial;
			}

				// Запрещаем выделение других объектов.
				HandleUtility.AddDefaultControl(GUIUtility.GetControlID(GetHashCode(), FocusType.Passive));				

		}

		// Обрабатываем события редактора.
		switch(Event.current.type) 
		{
		#region MouseMove
		case EventType.mouseMove:
		{
			m_currentObject = null;

			switch (m_editMode){

			case EditMode.addBlock:case EditMode.delBlock:{
					RaycastHit hitInfo = new RaycastHit();

					Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);

					if (Physics.Raycast(ray, out hitInfo)){

						if(m_editMode == EditMode.addBlock){

							Vector3 pointOnCollider = hitInfo.transform.position + hitInfo.normal * blockSize;
							intersectionPoint = pointOnCollider;										

						}else{

							m_currentObject = hitInfo.transform.gameObject;
							intersectionPoint = hitInfo.transform.position;
		
						}

						intersectionPoint.x = intersectionPoint.x - intersectionPoint.x % blockSize;
						intersectionPoint.y = intersectionPoint.y - intersectionPoint.y % blockSize;
						intersectionPoint.z = intersectionPoint.z - intersectionPoint.z % blockSize;

					}else{

						Plane hPlane = new Plane(Vector3.up, Vector3.zero);
						float distance = 0; 

						if (hPlane.Raycast(ray, out distance)){
							
							// make this local
							pointOnXY = ray.GetPoint(distance);																			
							
							intersectionPoint  = new Vector3(pointOnXY.x, pointOnXY.y, pointOnXY.z);

							intersectionPoint.x = intersectionPoint.x - intersectionPoint.x % blockSize;
							intersectionPoint.y = intersectionPoint.y - intersectionPoint.y % blockSize;
							intersectionPoint.z = intersectionPoint.z - intersectionPoint.z % blockSize;

							if (Physics.Raycast(pointOnXY, intersectionPoint, out hitInfo, Vector3.Distance(pointOnXY, intersectionPoint))){

								intersectionPoint = hitInfo.point;

								intersectionPoint.x = intersectionPoint.x - intersectionPoint.x % blockSize;
								intersectionPoint.y = intersectionPoint.y - intersectionPoint.y % blockSize;
								intersectionPoint.z = intersectionPoint.z - intersectionPoint.z % blockSize;

							}
						}
					}
	
					Event.current.Use();
					break;
				}
			}
			break;
		}
		#endregion
		
		#region MouseUp
		case EventType.mouseUp:{

			if (Event.current.button == 0)
			{
				if (!(m_editMode == EditMode.addBlock
				       || m_editMode == EditMode.delBlock)){
					break;
				}

				switch(m_editMode){
					case EditMode.addBlock:{

						GameObject CreatedCube;
						m_Instance.CreateCubeAt(intersectionPoint, out CreatedCube);

						Event.current.Use(); break;}

					case EditMode.delBlock:{

						if (m_currentObject){
							m_Instance.DeleteCubeAt(m_currentObject.transform.position);
						}

						Event.current.Use(); break;}
				}

			}

			break;
		}
		#endregion

		#region KeyUp
		case EventType.keyUp:
		{

			switch (Event.current.keyCode){
			
			case KeyCode.Escape:{

				DeselectThisGrid();

				Event.current.Use();
				break;
			}
			case KeyCode.Space:{

				SwitchState(); 

				Event.current.Use();
				break;
			}
			default: break;
			}


			break;
		}
		default: break;

		}
		#endregion
		DrawBlockMarker (blockSize, noRotation);

	}

	override public void OnInspectorGUI(){

		if (m_Library && (m_Instance.currentPrefab == null)){
			m_Instance.currentPrefab = m_Library.GetGameObjectByIndex(0);
			if (!m_Instance.currentPrefab){return;}
		}

		DrawDefaultInspector();
		
		EditorGUILayout.Separator();

	}
	
	#endregion


	#region Служебные процедуры

	private void DeleteHidenObjects(){

		CubeTagBehavior[] GameObjects = GameObject.FindObjectsOfType<CubeTagBehavior>();
		foreach(CubeTagBehavior go in GameObjects){
			m_Instance.DeleteCubeAt(go.transform.position);
			if(go != null){DestroyImmediate(go.gameObject);};
		}

	}

	private void DeselectThisGrid(){
		
		DestroyImmediate(m_marker);		
		Selection.objects = new UnityEngine.Object[0];
		
	}

	void DrawBlockMarker (float blockSize, Quaternion noRotation)
	{
		if (m_marker) {

			if(m_currentObject){
				m_marker.transform.localScale = new Vector3(blockSize, blockSize, blockSize);
				m_markerMaterial.color = Color.Lerp(Color.red, meshRenderer.sharedMaterial.color, 0.85f);
				m_marker.transform.localScale = m_marker.transform.localScale * (1 + m_marker.transform.localScale.x / 100.0f);
			}else{
				m_marker.transform.localScale = new Vector3(blockSize, blockSize, blockSize);
				m_markerMaterial.color = Color.Lerp(Color.green, meshRenderer.sharedMaterial.color, 0.85f);
			}

			m_marker.transform.position = intersectionPoint;
		}
	}
	
	private void SwitchState(){

		switch(m_editMode){

		case EditMode.addBlock:{
			m_editMode = EditMode.delBlock;
			break;
		}

		case EditMode.delBlock:{
			m_editMode = EditMode.addBlock;
			break;
		}
		}

		Repaint();

	}

	private void processSelectionGridMenu(SceneView _view){

		float ViewWidth  = 640;
		float ViewHeight = 480;

		if (_view != null){
			if (_view.camera != null){
				ViewWidth  = _view.camera.pixelWidth;
				ViewHeight = _view.camera.pixelHeight;
			}
		}

		Rect GridWindowRect = new Rect(10,ViewHeight / 2 - 100, 320, ViewHeight - 5 - (ViewHeight / 2 - 100));

		if(Event.current.type == EventType.layout){
			ShowCubeGrid = ShowCubeGridInternal;
		}
			if (ShowCubeGrid){
			
				ShowCubeGridInternal = GridWindowRect.Contains(Event.current.mousePosition);
		
			}else{
			
				GUILayout.BeginArea(new Rect(20, ViewHeight - 60, 100, ViewHeight - 50));				
				ShowCubeGridInternal = GUILayout.Button("Blocks");
				GUILayout.EndArea();
		
			}


			if (ShowCubeGrid){
			
				// + MORS
				// Тут есть ошибка.
				// Суть: OnInspectorGUI() вызывается для разных событий несколько раз. 
				// Если менеджер куи видит что между событием планировки интерфейса и событием ешо отрисовки
				// меняются элементы, он выкидывает исключение. На работу редактора не влияет, 1-2 кадра получаются без ГУИ - не критично.
				// Как исправить ошибку не понимаю - если есть идеи - велком.
				// - MORS
				GridWindowRect = GUILayout.Window(110, GridWindowRect, GridWindowProc, "Tiles:" + ViewWidth.ToString() + "x" + ViewHeight.ToString());
				return;
			}

	}

	void GridWindowProc(int windowID){

		scrollPosCubeGrid = GUILayout.BeginScrollView(scrollPosCubeGrid, true, true);
		
		Texture[] images = m_Library.GetImageList();
		System.Collections.Generic.KeyValuePair<string, GameObject>[] objects = m_Library.GetObjectsAndGuids();
		
		selectedCubeIndex = GUILayout.SelectionGrid(selectedCubeIndex, images, 2);
		m_Instance.currentPrefab = objects[selectedCubeIndex].Value;
		m_Instance.currentPrefabGuid = objects[selectedCubeIndex].Key;

		GUILayout.EndScrollView();
	}

	private bool processGUIMenus(SceneView _view){

		Rect _rect = new Rect(10, 10, 100, 400);

		GUILayout.BeginArea(_rect);

		int selectedGridIndex = 0;

		switch (m_editMode){
		case EditMode.addBlock:{
			selectedGridIndex = 0;
			break;
		}
		case EditMode.delBlock:{
			selectedGridIndex = 1;
			break;
		}
		}

		string[] _buttons = new string[]{"Add", "Delete", "Clear", "Destroy hiden", "Save", "Load" , "Exit"};

		int oldButtonSelected = selectedGridIndex;

		selectedGridIndex = GUILayout.SelectionGrid(selectedGridIndex, _buttons , 1);

		GUILayout.EndArea();

		processSelectionGridMenu(_view);

		if(oldButtonSelected != selectedGridIndex){

			switch (selectedGridIndex){
			case 0:{
				m_editMode = EditMode.addBlock;
				break;
			}
			case 1:{
				m_editMode = EditMode.delBlock;
				break;
			}
			case 2:{
				m_Instance.ClearDictionary();
				break;
			}

			case 3:{
				DeleteHidenObjects();
				break;
			}

			case 4:{

				string path = EditorUtility.SaveFilePanelInProject("Select level path","Level", "xml", "Select a file");				
				if (path.Length != 0){
					m_Instance.SerializeMe(path);
				}
				break;
			}
			case 5:{
				string path = EditorUtility.OpenFilePanel("Select level path",Application.dataPath, "xml");				
				if (path.Length != 0){
					m_Instance.ClearDictionary();
					ScriptableObject.DestroyImmediate(m_Instance);
					m_Instance = CubeGridXML.ToGrid(path);

					CubeGridSingletonObject sing = GameObject.FindObjectOfType<CubeGridSingletonObject>();
					if (sing != null){
						sing.Grid = m_Instance;
					}

				}
				break;
			}
			case 6:{
				DeselectThisGrid();
				break;
			}
			}

			return true;
		};

		return false;
	
	}

	#endregion
}