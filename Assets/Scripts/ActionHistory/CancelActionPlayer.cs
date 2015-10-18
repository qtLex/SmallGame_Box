using UnityEngine;
using System.Collections;
using BoxClasses;
using GameEnums;

public class CancelActionPlayer : BaseCancelAction {

	public override bool CancelAction(ActionHistoryType type, Vector3 lastDirection){

		PlayerController controller = GetComponent<PlayerController>();

		if(!controller)
			return false;

        return controller.CalculateMovement(-lastDirection, false);

	}

}
