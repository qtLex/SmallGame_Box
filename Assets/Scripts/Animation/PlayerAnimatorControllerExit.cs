using UnityEngine;
using System.Collections;

public class PlayerAnimatorControllerExit : StateMachineBehaviour {

	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {

		PlayerController playerController = animator.gameObject.GetComponent<PlayerController>();
		playerController.DragMainPivot();

		MovingBox movingBox = GlobalOptions.CurrentBox.GetComponent<MovingBox>();

		if(!movingBox)
			return;

		movingBox.isMovingFalse();

	}

}
