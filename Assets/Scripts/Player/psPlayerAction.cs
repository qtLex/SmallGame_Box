using UnityEngine;
using System.Collections;
using System;

public class psPlayerAction : MonoBehaviour {

	private ParticleSystem psVer;
	private ParticleSystem psHor;

	public GameObject gpsver;
	public GameObject gpshor;

	// Use this for initialization
	void Awake(){
		Messenger.AddListener("UserAction", UserAction);

		psVer = gpsver.GetComponent<ParticleSystem>();
		psHor = gpshor.GetComponent<ParticleSystem>();
	}
	
	public void UserAction(object sender, EventArgs evArgs){

		//Debug.Log("Проверка");

		if(transform.up == Vector3.up || transform.up == -Vector3.up){
			if(!psHor.isPlaying)
				psHor.Play();
		}
		else{
			if(!psVer.isPlaying)
				psVer.Play();
		}
	}
}
