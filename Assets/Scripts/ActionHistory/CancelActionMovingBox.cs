using UnityEngine;
using System.Collections;
using BoxClasses;
using GameEnums;

public class CancelActionMovingBox : BaseCancelAction {

	public override bool CancelAction(ActionHistoryType type){
		
		GameObject player = GlobalOptions.Player;
		
		// проверим можно ли двигаться в указаном направлении
		Vector3 direction = player.transform.up;
		
		// двигаем объект и игрока
		float coef = GlobalOptions.Grid.gridSize;
		
		transform.position = new Vector3(transform.position.x + coef*direction.x,
		                                 transform.position.y + coef*direction.y,
		                                 transform.position.z + coef*direction.z);
		
		player.transform.position = new Vector3(player.transform.position.x + coef*direction.x,
		                                        player.transform.position.y + coef*direction.y,
		                                        player.transform.position.z + coef*direction.z);

		// установим тригеры аниматора
		string triggerName = "";
		if(direction == transform.forward)       triggerName = "Forward";
		else if(direction == -transform.forward) triggerName = "Back";
		else if(direction == transform.up)       triggerName = "Top";
		else if(direction == -transform.up)      triggerName = "Bottom";
		else if(direction == transform.right)    triggerName = "Right";
		else if(direction == -transform.right)   triggerName = "Left";
		
		if(triggerName != "") GetComponent<Animator>().SetTrigger(triggerName);
		
		Animator pAnimator = player.GetComponent<Animator>();
		pAnimator.SetTrigger("UserActionNegative");
		
		return true;
	}

}
