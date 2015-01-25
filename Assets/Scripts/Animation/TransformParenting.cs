using UnityEngine;
using System.Collections;

public class TransformParenting : MonoBehaviour {

	public Transform ParentTransform;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		transform.position = ParentTransform.position;
		transform.rotation = ParentTransform.rotation;
	
	}
}
