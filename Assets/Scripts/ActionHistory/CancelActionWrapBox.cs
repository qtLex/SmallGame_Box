using UnityEngine;
using System.Collections;
using BoxClasses;
using GameEnums;
using Motors.BoxMotors;

public class CancelActionWrapBox : BaseCancelAction
{
    private BoxMotor motor;
    private CubeGrid _grid;
    private GameObject _player;

    void Awake()
    {
        _grid = GlobalOptions.Grid;
        _player = GlobalOptions.Player;
        motor = GetComponent<BoxMotor>();
    }
    
    public override bool CancelAction(ActionHistoryType type, Vector3 lastDirection)
    {
        // проверим возможность передвижения
        if (lastDirection == Vector3.zero)
            return false;

        int layerMask = 1 << LayerMask.NameToLayer("Box");
        Vector3 directionUp = (_player.transform.up);

        if (Physics.Raycast(transform.position, lastDirection, _grid.gridSize, layerMask))
        {
            Vector3 FirstPosition = new Vector3(directionUp.x * _grid.gridSize + transform.position.x, directionUp.y * _grid.gridSize + transform.position.y, directionUp.z * _grid.gridSize + transform.position.z);
            Vector3 SecondPosition = new Vector3(lastDirection.x * _grid.gridSize + FirstPosition.x, lastDirection.y * _grid.gridSize + FirstPosition.y, lastDirection.z * _grid.gridSize + FirstPosition.z);
            Vector3[] path = { FirstPosition, SecondPosition };
            motor.path = path;
        }
        else
        {
            Vector3 FirstPosition = new Vector3(lastDirection.x * _grid.gridSize + transform.position.x, lastDirection.y * _grid.gridSize + transform.position.y, lastDirection.z * _grid.gridSize + transform.position.z);
            Vector3[] path = { FirstPosition };
            motor.path = path;
        }
        
        motor.StartMoving();

        return true;
    }
}
