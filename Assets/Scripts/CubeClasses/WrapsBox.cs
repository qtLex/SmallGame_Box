using UnityEngine;
using System;
using System.Collections;
using BoxClasses;
using GameEnums;
using Motors.BoxMotors;

public class WrapsBox : BaseBox
{
    private CubeGrid   _grid;
    private GameObject _player;

    private BoxMotor motor;

    void Awake()
    {
        _grid   = GlobalOptions.Grid;
        _player = GlobalOptions.Player;

        motor = GetComponent<BoxMotor>();
    }

    private bool CanMove(out Vector3 directionMove) 
    {     
        int layerMask = 1 << LayerMask.NameToLayer("Box");
        Vector3 direction = -(_player.transform.up);

        if (!Physics.Raycast(transform.position, direction, _grid.gridSize, layerMask))
        {
            directionMove = Vector3.zero;
            return false;
        }

        // направления по чавой стрелке от направления взгляда игрока
        Vector3[] playerDirections = { _player.transform.forward, _player.transform.right, -(_player.transform.forward), -(_player.transform.right)}; 

        // проверим наличие кубов по бокам
        foreach (Vector3 playerDirection in playerDirections)
        {

            if (!Physics.Raycast(transform.position, playerDirection, _grid.gridSize, layerMask))
            {
                directionMove = playerDirection;
                return true;
            }
        }

        // двигаться нельзя
        directionMove = Vector3.zero;
        return false;
    }

    public override void UserAction(object sender, EventArgs evArgs)
    {
        if (!motor && motor.isMoving)
            return;

        Vector3 direction = Vector3.zero;

        // проверим возможность передвижения
        if (!CanMove(out direction) || direction == Vector3.zero)
            return;

        Vector3 FirstPosition = new Vector3(direction.x * _grid.gridSize + transform.position.x, direction.y * _grid.gridSize + transform.position.y, direction.z * _grid.gridSize + transform.position.z);


        int layerMask = 1 << LayerMask.NameToLayer("Box");
        Vector3 directionBottom = -(_player.transform.up);


        if (!Physics.Raycast(FirstPosition, directionBottom, _grid.gridSize, layerMask))
        {
            Vector3 SecondPosition = new Vector3(directionBottom.x * _grid.gridSize + FirstPosition.x, directionBottom.y * _grid.gridSize + FirstPosition.y, directionBottom.z * _grid.gridSize + FirstPosition.z);
            Vector3[] path = { FirstPosition, SecondPosition };
            motor.path = path;
        }
        else
        {
            Vector3[] path = { FirstPosition };
            motor.path = path;
        }


        ActionHistory.ActionHistoryManager.AddToHistory(ActionHistoryType.Direction, this.gameObject, -direction);
        
        motor.StartMoving();
    }

    public override void OnRetire()
    {
        thisAnimator.SetBool("Approach", false);

    }

    public override void OnApproach()
    {
        thisAnimator.SetBool("Approach", true);

    }
}
