using UnityEngine;
using System.Collections;

public class AlignToView : MonoBehaviour {

	private Camera _camera;

	void Start () {
		_camera = Camera.main;
	}

	void Update()
	{
		transform.LookAt(transform.position + _camera.transform.rotation * Vector3.back,
		                 _camera.transform.rotation * Vector3.up);
	}

}
