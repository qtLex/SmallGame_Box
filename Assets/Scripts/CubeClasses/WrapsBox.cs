using UnityEngine;
using System;
using System.Collections;
using BoxClasses;
using GameEnums;

public class WrapsBox : BaseBox
{
    private bool       _isMoving = false;
    private CubeGrid   _grid;
    private GameObject _player;

    public void isMovingFalse()
    {
        _isMoving = false;
    }

    void Awake()
    {
        _grid = GlobalOptions.Grid;
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

        // направления по чавой стрелке от направления взгляда
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
        Vector3 direction = Vector3.zero;

        // проверим возможность передвижения
        if (!CanMove(out direction) || direction == Vector3.zero)
            return;

        // получим направление наименование тригера
        string nameTriger = "";
        if (direction == transform.forward)
            nameTriger = "forward";
        else if (direction == transform.right)
            nameTriger = "right";
        else if (direction == -transform.forward)
            nameTriger = "back";
        else if (direction == -transform.right)
            nameTriger = "left";

        // установим тригер
        thisAnimator.SetTrigger(nameTriger);



        // обновляем аниматор
        thisAnimator.Update(Time.deltaTime);

    }
}
