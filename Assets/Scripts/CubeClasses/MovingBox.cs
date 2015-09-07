using UnityEngine;
using System;
using System.Collections;
using BoxClasses;
using GameEnums;
using Motors.BoxMotors;

public class MovingBox : BaseBox
{
	private CubeGrid _grid;
    private GameObject _player;
    private BoxMotor _motor;

	//public static GameObject FieldPrefab;
    //private GameObject FieldInstance;
	//private bool       _isMoving = false;

	//public void isMovingFalse(){
	//	_isMoving = false;
	//}

	// Use this for initialization
	void Awake () 
	{
		_grid = GlobalOptions.Grid;
        _player = GlobalOptions.Player;

        _motor = GetComponent<BoxMotor>();
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
		return !Physics.Raycast(transform.position, direction, _grid.gridSize, layerMask);
	}

	public override void UserAction(object sender, EventArgs evArgs)
	{
        if (!_motor && _motor.isMoving)
            return;

		// проверим можно ли двигаться в указаном направлении
		Vector3 direction = -_player.transform.up;

		if(!CanMove() || !CanMove(direction))
			return;

		// двигаем объект и игрока
		float coef = _grid.gridSize;

        Vector3 NewPosition = new Vector3(transform.position.x + coef * direction.x,
		                                 transform.position.y + coef*direction.y,
		                                 transform.position.z + coef*direction.z);

        Vector3[] path = { NewPosition };
        _motor.path = path;

        ActionHistory.ActionHistoryManager.AddToHistory(ActionHistoryType.Direction, this.gameObject, -direction);

        _motor.StartMoving();

	}

	public override void OnRetire(){

		thisAnimator.SetBool("Approach", false);

	}

	public override void OnApproach(){

		thisAnimator.SetBool("Approach", true);
	
	}

}
