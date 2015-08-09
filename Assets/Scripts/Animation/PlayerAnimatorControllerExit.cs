using UnityEngine;
using System.Collections;

public class PlayerAnimatorControllerExit : StateMachineBehaviour {

	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		Debug.Log("exit");

		PlayerController playerController = animator.gameObject.GetComponent<PlayerController>();
		playerController.DragMainPivot();
	}

}
