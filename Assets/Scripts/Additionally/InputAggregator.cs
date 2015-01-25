using UnityEngine;
using System.Collections;
using GameEnums;
using BoxClasses;

public class InputAggregator : MonoBehaviour {

	public GameObject prefPlayer;

	// Use this for initialization
	void Start () {
		GameObject startpoint = GameObject.Find("StartPoint(Clone)");

		if(!startpoint)
			return;

		GlobalOptions.Player.transform.position = startpoint.transform.position;
		GlobalOptions.Player.transform.rotation = startpoint.transform.rotation;
	}
	
	// Update is called once per frame
	void Update (){

		if(Input.GetKeyDown(KeyCode.Escape)){

		}
		else if(Input.GetKeyDown(KeyCode.UpArrow)
		        && GlobalOptions.Player != null){
			GlobalOptions.Player.GetComponent<PlayerController>().MovingKeyDown(KeyCode.UpArrow);
		}

		else if(Input.GetKeyDown(KeyCode.DownArrow)
		     	&& GlobalOptions.Player != null){
			GlobalOptions.Player.GetComponent<PlayerController>().MovingKeyDown(KeyCode.DownArrow);
		}

		else if(Input.GetKeyDown(KeyCode.RightArrow)
		     	&& GlobalOptions.Player != null){
			GlobalOptions.Player.GetComponent<PlayerController>().MovingKeyDown(KeyCode.RightArrow);
		}

		else if(Input.GetKeyDown(KeyCode.LeftArrow)
		        && GlobalOptions.Player != null){
			GlobalOptions.Player.GetComponent<PlayerController>().MovingKeyDown(KeyCode.LeftArrow);
		}

		else if(Input.GetKeyDown(KeyCode.B))
			ActionHistory.ActionHistoryManager.CancelLastAction();

		else if(Input.GetKeyDown(KeyCode.Space)
		        && GlobalOptions.CurrentBox != null){
			Messenger.Invoke("UserAction", this);
		}
	
	}
}
