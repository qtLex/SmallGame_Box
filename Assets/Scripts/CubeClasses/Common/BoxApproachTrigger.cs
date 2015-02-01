using UnityEngine;
using System.Collections;
using BoxClasses;

public class BoxApproachTrigger : MonoBehaviour {

	private Transform playerTransform;
	public float triggerDistance;

	// Use this for initialization
	void Start () {
		playerTransform = GlobalOptions.Player.transform;
	}
	
	// Update is called once per frame
	void Update () {

		float dist = Vector3.Distance(transform.position, playerTransform.position);

		if (dist <= triggerDistance){
			BaseBox boxComponent = GetComponent<BaseBox>();
			boxComponent.OnApproach();
		}else{
			BaseBox boxComponent = GetComponent<BaseBox>();
			boxComponent.OnRetire();
		}
	
	}
}
