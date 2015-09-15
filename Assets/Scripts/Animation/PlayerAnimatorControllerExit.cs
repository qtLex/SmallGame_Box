using UnityEngine;
using System.Collections;
using BoxClasses;

public class PlayerAnimatorControllerExit : StateMachineBehaviour {

	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {

		PlayerController playerController = animator.gameObject.GetComponent<PlayerController>();
		playerController.DragMainPivot();
       
	}

}
