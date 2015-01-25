using UnityEngine;
using System;
using System.Collections;
using BoxClasses;
using GameEnums;

public class MovingBox : BaseBox
{
	private CubeGrid Grid;
	public static GameObject FieldPrefab;

	private GameObject FieldInstance;
	
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
		// проверим есть ли рядом хотябы один объект
		if(!CanMove())
			return;

		GameObject player = GlobalOptions.Player;

		// проверим можно ли двигаться в указаном направлении
		Vector3 direction = -player.transform.up;

		if(!CanMove(direction))
			return;

		// двигаем объект и игрока
		float coef = Grid.gridSize;

		transform.position = new Vector3(transform.position.x + coef*direction.x,
		                                 transform.position.y + coef*direction.y,
		                                 transform.position.z + coef*direction.z);

		player.transform.position = new Vector3(player.transform.position.x + coef*direction.x,
		                                        player.transform.position.y + coef*direction.y,
		                                        player.transform.position.z + coef*direction.z);
	
		ActionHistory.ActionHistoryManager.AddToHistory(ActionHistoryType.Empty, this.gameObject);
	}

	public override void OnRetire(){

		thisAnimator.SetBool("Approach", false);

	}

	public override void OnApproach(){

		thisAnimator.SetBool("Approach", true);
	
	}

}
