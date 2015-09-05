﻿using UnityEngine;
using System;
using System.Collections;
using BoxClasses;
using GameEnums;

public class MovingBox : BaseBox
{
	private CubeGrid Grid;
	public static GameObject FieldPrefab;

	private GameObject FieldInstance;
	private bool       _isMoving = false;

	public void isMovingFalse(){
		_isMoving = false;
	}

	// Use this for initialization
	void Awake () 
	{
		Grid = GlobalOptions.Grid;
	}

	private bool CanMove()
	{
		if(CanMove(transform.right) && CanMove(-transform.right) && CanMove(transform.up) && CanMove(-transform.up) && CanMove(transform.forward) && CanMove(-transform.forward))
			return false;

		return true;
	}

	private bool CanMove(Vector3 direction)
	{
		int layerMask = 1 << LayerMask.NameToLayer("Box");
		return !Physics.Raycast(transform.position, direction, Grid.gridSize, layerMask);
	}

	public override void UserAction(object sender, EventArgs evArgs)
	{
        if (_isMoving)
            return;
        
        GameObject player = GlobalOptions.Player;

		if(player.GetComponent<PlayerController>().isMoving()) return;

		// проверим есть ли рядом хотябы один объект
		if(!CanMove())
			return;

		// проверим можно ли двигаться в указаном направлении
		Vector3 direction = -player.transform.up;

		if(!CanMove(direction))
			return;

		// двигаем объект и игрока
		float coef = Grid.gridSize;

		transform.position = new Vector3(transform.position.x + coef*direction.x,
		                                 transform.position.y + coef*direction.y,
		                                 transform.position.z + coef*direction.z);

		PlayerController playerController = player.GetComponent<PlayerController>();

		playerController.NewPoint = new Vector3(player.transform.position.x + coef*direction.x,
		                                        player.transform.position.y + coef*direction.y,
		                                        player.transform.position.z + coef*direction.z);
	
		ActionHistory.ActionHistoryManager.AddToHistory(ActionHistoryType.Empty, this.gameObject, Vector3.zero);

		// установим тригеры аниматора
		string triggerName = "";
		if(direction == transform.forward)       triggerName = "Forward";
		else if(direction == -transform.forward) triggerName = "Back";
		else if(direction == transform.up)       triggerName = "Top";
		else if(direction == -transform.up)      triggerName = "Bottom";
		else if(direction == transform.right)    triggerName = "Right";
		else if(direction == -transform.right)   triggerName = "Left";

		if(triggerName != "") thisAnimator.SetTrigger(triggerName);
		Animator pAnimator = player.GetComponent<Animator>();
		pAnimator.SetTrigger("UserAction");

		_isMoving = true;

		pAnimator.Update(Time.deltaTime);
		thisAnimator.Update(Time.deltaTime);

	}

	public override void OnRetire(){

		if(_isMoving)
			return;

		thisAnimator.SetBool("Approach", false);

	}

	public override void OnApproach(){

		if(_isMoving)
			return;

		thisAnimator.SetBool("Approach", true);
	
	}

}
