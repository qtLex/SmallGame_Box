using UnityEngine;
using System.Collections;
using BoxClasses;
using GameEnums;

public class CancelActionPlayer : BaseCancelAction {

	public override bool CancelAction(ActionHistoryType type, Vector3 lastDirection){

		PlayerController controller = GetComponent<PlayerController>();

		if(!controller)
			return false;

		switch (type){
		case ActionHistoryType.Forward:
			return controller.MovingKeyDown(KeyCode.DownArrow, false);
		case ActionHistoryType.Back:
			return controller.MovingKeyDown(KeyCode.UpArrow, false);
		case ActionHistoryType.Left:
			return controller.MovingKeyDown(KeyCode.RightArrow, false);
		case ActionHistoryType.Right:
			return controller.MovingKeyDown(KeyCode.LeftArrow, false);
		default:
			return false;
		}

	}

}
