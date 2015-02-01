using UnityEngine;
using System.Collections;
using GameEnums;


public class PlayerController : MonoBehaviour {

	#region Private

	private CubeGrid   Grid;
	private GameObject CurrentBox;
	private Animator animator;

	#endregion

	public bool UseAnimationOnCancel = false;

	// Use this for initialization
	// delete
	void Awake () 
	{
		Grid = GlobalOptions.Grid;
		animator = GetComponent<Animator>();
	}

	public bool isMoving(){
		return !animator.GetCurrentAnimatorStateInfo(0).IsName("PStay");
	}

	public bool MovingKeyDown(KeyCode key, bool bakehistory = true){

		if(isMoving()) return false;

		Vector3 direction = Vector3.zero;

		switch (key){
		case KeyCode.UpArrow:
			direction = transform.forward;
			animator.SetTrigger("UpArrow");
			if(bakehistory)
				ActionHistory.ActionHistoryManager.AddToHistory(ActionHistoryType.Forward, this.gameObject);
		break;
		case KeyCode.DownArrow:
			direction = -transform.forward;
			animator.SetTrigger("DownArrow");
			if(bakehistory)
				ActionHistory.ActionHistoryManager.AddToHistory(ActionHistoryType.Back, this.gameObject);
		break;
		case KeyCode.LeftArrow:
			direction = -transform.right;
			animator.SetTrigger("LeftArrow");
			if(bakehistory)
				ActionHistory.ActionHistoryManager.AddToHistory(ActionHistoryType.Left, this.gameObject);
		break;
		case KeyCode.RightArrow:
			direction = transform.right;
			animator.SetTrigger("RightArrow");
			if(bakehistory)
				ActionHistory.ActionHistoryManager.AddToHistory(ActionHistoryType.Right, this.gameObject);
		break;
		default:
			return false;
		}

		// определяем препятствия
		RaycastHit hit = new RaycastHit();
		int layerMask = 1 << LayerMask.NameToLayer("Box");
		if(Physics.Raycast(transform.position, direction, out hit, Grid.gridSize, layerMask)){

			// выполним поворот
			float coefForward = -90*Vector3.Dot(direction,transform.forward);
			float coefRight   = 90*Vector3.Dot(direction,transform.right);

			if(coefForward != 0)
				transform.Rotate(transform.right, coefForward, Space.World);
			
			if(coefRight != 0)
				transform.Rotate(transform.forward, coefRight, Space.World);

			if(bakehistory || UseAnimationOnCancel)
				animator.SetTrigger("MoveUp");

		}
		else if(Physics.Raycast(GlobalOptions.CurrentBox.transform.position, direction, out hit, Grid.gridSize, layerMask)){

			transform.position = transform.position + direction*Grid.gridSize;
			if(bakehistory || UseAnimationOnCancel)
				animator.SetTrigger("Move");

		}
		else{
			Vector3 newPoint = new Vector3(transform.position.x + Grid.gridSize*(-transform.up).x + Grid.gridSize*direction.x,
			                               transform.position.y + Grid.gridSize*(-transform.up).y + Grid.gridSize*direction.y,
			                               transform.position.z + Grid.gridSize*(-transform.up).z + Grid.gridSize*direction.z);
			
			transform.position = newPoint;
			
			float coefForward = 90*Vector3.Dot(direction,transform.forward);
			float coefRight   = -90*Vector3.Dot(direction,transform.right);
			
			if(coefForward != 0)
				transform.Rotate(transform.right, coefForward, Space.World);
			
			if(coefRight != 0)
				transform.Rotate(transform.forward, coefRight, Space.World);
			if(bakehistory || UseAnimationOnCancel)
				animator.SetTrigger("MoveDown");
		}

		return true;
	}	
}
