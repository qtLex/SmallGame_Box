using UnityEngine;
using System.Collections;

//[RequireComponent(typeof(PlayerController))]
public class PlayerCameraAlign : MonoBehaviour {

	Transform MainCameraTransform;
	PlayerController playerComntroller;
	float changeDelay = 0.2f;
	float currentDelay = 0;

	void Start () {
		MainCameraTransform = Camera.main.transform;
		playerComntroller = GetComponent<PlayerController>();
	}

	void Update () {

		if (GlobalOptions.isEditMode) return;
		if (playerComntroller.isMoving()) return;
		
		Vector3 playerDirection = transform.forward;
		Vector3 cameraDirection = Vector3.ProjectOnPlane(MainCameraTransform.forward, transform.up).normalized;

		float dotCameraPlayer = Vector3.Dot(playerDirection, cameraDirection);
		currentDelay -= Time.deltaTime;

		if(dotCameraPlayer < 0.5f){

			if (currentDelay <= 0){

				Vector3 newForward = Vector3.Cross(playerDirection, transform.up);			

				if (Vector3.Dot(newForward, cameraDirection) > 0.5f){

					Debug.DrawLine(transform.position, transform.position + newForward * 10, Color.red, 1.0f);

					transform.LookAt(newForward + transform.position, transform.up);
					currentDelay = changeDelay;
							
				}else{

					Debug.DrawLine(transform.position, -newForward * 10 + transform.position, Color.red, 1.0f);

					transform.LookAt(-newForward + transform.position, transform.up);
					currentDelay = changeDelay;

				};

			};
		};
	}
}
