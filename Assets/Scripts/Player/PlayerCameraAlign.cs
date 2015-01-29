using UnityEngine;
using System.Collections;

public class PlayerCameraAlign : MonoBehaviour {

	Transform MainCameraTransform;
	// Use this for initialization
	void Start () {
		MainCameraTransform = Camera.main.transform;
	}
	
	// Update is called once per frame
	void Update () {
	
		Vector3 playerDirection = new Vector3(transform.forward.x, 0, transform.forward.z).normalized;
		Vector3 cameraDirection = new Vector3(MainCameraTransform.forward.x, 0, MainCameraTransform.forward.z).normalized;

		float dotCameraPlayer = Vector3.Dot(playerDirection, cameraDirection);

		float angle = Vector3.Angle(playerDirection,cameraDirection);

		if(dotCameraPlayer < 0.5f){

			Vector3 newForward = Vector3.Cross(playerDirection, Vector3.up);
			if (Vector3.Dot(newForward, cameraDirection) > 0.5f){
				transform.forward = newForward;
			}else{
				transform.forward = -newForward;
			};

		}

	}

	Vector3 SnapTo(Vector3 v3, float snapAngle) {
		float   angle = Vector3.Angle (v3, Vector3.up);
		if (angle < snapAngle / 2.0f)          // Cannot do cross product 
			return Vector3.up * v3.magnitude;  //   with angles 0 & 180
		if (angle > 180.0f - snapAngle / 2.0f)
			return Vector3.down * v3.magnitude;
		
		float t = Mathf.Round(angle / snapAngle);
		
		float deltaAngle = (t * snapAngle) - angle;
		
		Vector3 axis = Vector3.Cross(Vector3.up, v3);
		Quaternion q = Quaternion.AngleAxis (deltaAngle, axis);
		return q * v3;
	}
}
