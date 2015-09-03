﻿using UnityEngine;
using System.Collections;
using GameEnums;
using BoxClasses;

public class InputAggregator : MonoBehaviour {

	public GameObject prefPlayer;
	private PlayerController _playerController;

	// Use this for initialization
	void Start () {
		GameObject startpoint = GameObject.Find("StartPoint(Clone)");
		_playerController = GlobalOptions.Player.GetComponent<PlayerController>();

		if(!startpoint)
			return;

		GlobalOptions.Player.transform.position = startpoint.transform.position;
		GlobalOptions.Player.transform.rotation = startpoint.transform.rotation;
		
	}
	
	// Update is called once per frame
	void Update (){

		// + mors
		// Изменение режима
		if(Input.GetKeyUp(KeyCode.F12)){
			GlobalOptions.SwitchMode();
		}
		// - mors

		if (GlobalOptions.isPlayMode){
			PlayModeControl();
			return;
		}

		if (GlobalOptions.isEditMode){
			EditModeControl();
			return;
		}
	
	}

	private void PlayModeControl(){

		if(Input.GetKeyDown(KeyCode.UpArrow)
		   && _playerController != null){
			_playerController.MovingKeyDown(KeyCode.UpArrow);
		}
		
		else if(Input.GetKeyDown(KeyCode.DownArrow)
		        && _playerController != null){
			_playerController.MovingKeyDown(KeyCode.DownArrow);
		}
		
		else if(Input.GetKeyDown(KeyCode.RightArrow)
		        && _playerController != null){
			_playerController.MovingKeyDown(KeyCode.RightArrow);
		}
		
		else if(Input.GetKeyDown(KeyCode.LeftArrow)
		        && _playerController != null){
			_playerController.MovingKeyDown(KeyCode.LeftArrow);
		}
		
		else if(Input.GetKeyDown(KeyCode.B))
			ActionHistory.ActionHistoryManager.CancelLastAction();
		
		else if(Input.GetKeyDown(KeyCode.Space)
		        && GlobalOptions.CurrentBox != null){
			Messenger.Invoke("UserAction", this);
		}
		
	}

	private void EditModeControl(){

		if(Input.GetKeyDown(KeyCode.Space)){
			
			Messenger.Invoke("ShowHideBoxSelection", this);

		}
		else if(Input.GetKeyUp(KeyCode.Space)){

			Messenger.Invoke("ShowHideBoxSelection", this);

		}

		if (Input.GetMouseButtonUp(0)){
			Messenger.Invoke("MouseUp", this, new GlobalOptions.MouseButtonsEventArgs(0));
		}else if(Input.GetMouseButtonDown(0)){
			Messenger.Invoke("MouseDown", this, new GlobalOptions.MouseButtonsEventArgs(0));
		}else if(Input.GetMouseButton(0)){
			Messenger.Invoke("MouseHold", this, new GlobalOptions.MouseButtonsEventArgs(0));
		}

		if (Input.GetMouseButtonUp(1)){
			Messenger.Invoke("MouseUp", this, new GlobalOptions.MouseButtonsEventArgs(1));
		}else if(Input.GetMouseButtonDown(1)){
			Messenger.Invoke("MouseDown", this, new GlobalOptions.MouseButtonsEventArgs(1));
		}else if(Input.GetMouseButton(1)){
			Messenger.Invoke("MouseHold", this, new GlobalOptions.MouseButtonsEventArgs(1));
		}
			
	}

}

