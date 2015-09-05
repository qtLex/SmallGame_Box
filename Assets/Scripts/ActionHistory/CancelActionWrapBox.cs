using UnityEngine;
using System.Collections;
using BoxClasses;
using GameEnums;
using Motors.BoxMotors;

public class CancelActionWrapBox : BaseCancelAction
{
    private BoxMotor motor;

    private CubeGrid _grid;

    void Awake()
    {
        _grid = GlobalOptions.Grid;
        
        motor = GetComponent<BoxMotor>();
    }
    
    public override bool CancelAction(ActionHistoryType type, Vector3 lastDirection)
    {
        Debug.Log("Cancel: " + lastDirection);

        // проверим возможность передвижения
        if (lastDirection == Vector3.zero)
            return false;

        Vector3 NewPosition = new Vector3(lastDirection.x * _grid.gridSize + transform.position.x, lastDirection.y * _grid.gridSize + transform.position.y, lastDirection.z * _grid.gridSize + transform.position.z);
        Vector3[] path = { NewPosition };
        motor.path = path;

        motor.StartMoving();

        return true;
    }
}
