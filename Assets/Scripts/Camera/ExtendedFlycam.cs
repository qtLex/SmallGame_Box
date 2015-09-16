using UnityEngine;
using System;
using System.Collections;

public class ExtendedFlycam : MonoBehaviour
{

	public float cameraSensitivity = 90;
	public float climbSpeed = 4;
	public float normalMoveSpeed = 10;
	public float slowMoveFactor = 0.25f;
	public float fastMoveFactor = 3;

	public bool RotateByMouse = false;
	
	private float _rotationX = 0.0f;
	private float _rotationY = 0.0f;
	private bool _active = true;

	private CubeGridEditorGameMode _editorMode;
	
	void OnEnable(){
		if(RotateByMouse){
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
		}
		transform.LookAt(GlobalOptions.Player.transform, transform.up);
		Messenger.AddListener("ShowHideBoxSelection", ShowHideBoxSelectionMenu);  

		_editorMode = GlobalOptions.GetEditorComponent();

	}

	void OnDisable(){
		
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
		
		Messenger.RemoveListener("ShowHideBoxSelection", ShowHideBoxSelectionMenu);
	}
	
	private void ShowHideBoxSelectionMenu(object sender, EventArgs evArgs){

		Cursor.lockState = (Cursor.lockState == CursorLockMode.None) ?  CursorLockMode.Locked : CursorLockMode.None;
		Cursor.visible = !Cursor.visible;
		_active = !_active;

	}
	
	void Update ()
	{
		if (!_active) return;

		if(RotateByMouse){
			_rotationX += Input.GetAxis("Mouse X") * cameraSensitivity * Time.deltaTime;
			_rotationY += Input.GetAxis("Mouse Y") * cameraSensitivity * Time.deltaTime;
			_rotationY = Mathf.Clamp (_rotationY, -90, 90);

			transform.localRotation = Quaternion.AngleAxis(-_rotationX, Vector3.up);
			transform.localRotation *= Quaternion.AngleAxis(_rotationY, Vector3.left);
		}else{

			if(Input.GetMouseButton(1)){

				_editorMode.TraceMouse = false;

				GameObject _marker = _editorMode.CursorObject;
				Vector3 HorizontalAxis = Vector3.Cross(transform.forward, _marker.transform.up);

				transform.RotateAround(_marker.transform.position, _marker.transform.up, (normalMoveSpeed * fastMoveFactor) * Input.GetAxis("Mouse X") * Time.deltaTime);
				transform.RotateAround(_marker.transform.position, HorizontalAxis, (normalMoveSpeed * fastMoveFactor) * Input.GetAxis("Mouse Y") * Time.deltaTime);
				transform.Translate(new Vector3(0,0,1) * normalMoveSpeed * Input.GetAxis("Mouse ScrollWheel"));

				return;

			}else{

				_editorMode.TraceMouse = true;

			}

		}		
		
		if (Input.GetKey (KeyCode.LeftShift) || Input.GetKey (KeyCode.RightShift))
		{
			transform.position += transform.forward * (normalMoveSpeed * fastMoveFactor) * Input.GetAxis("Vertical") * Time.deltaTime;
			transform.position += transform.right * (normalMoveSpeed * fastMoveFactor) * Input.GetAxis("Horizontal") * Time.deltaTime;
		}
		else if (Input.GetKey (KeyCode.LeftControl) || Input.GetKey (KeyCode.RightControl))
		{
			transform.position += transform.forward * (normalMoveSpeed * slowMoveFactor) * Input.GetAxis("Vertical") * Time.deltaTime;
			transform.position += transform.right * (normalMoveSpeed * slowMoveFactor) * Input.GetAxis("Horizontal") * Time.deltaTime;
		}
		else
		{
			transform.position += transform.forward * normalMoveSpeed * Input.GetAxis("Vertical") * Time.deltaTime;
			transform.position += transform.right * normalMoveSpeed * Input.GetAxis("Horizontal") * Time.deltaTime;
		}
		
		
		if (Input.GetKey (KeyCode.Q)) {transform.position += transform.up * climbSpeed * Time.deltaTime;}
		if (Input.GetKey (KeyCode.E)) {transform.position -= transform.up * climbSpeed * Time.deltaTime;}
		
		if (Input.GetKeyDown (KeyCode.End))
		{
			Cursor.lockState = (Cursor.lockState == CursorLockMode.None) ?  CursorLockMode.Locked : CursorLockMode.None;
		}
	}
}