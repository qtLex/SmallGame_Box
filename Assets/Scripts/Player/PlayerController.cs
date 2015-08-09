using UnityEngine;
using System.Collections;
using GameEnums;


public class PlayerController : MonoBehaviour {

	#region Private

	private CubeGrid   Grid;
	private GameObject CurrentBox;
	private Animator   animator;
	private Animator   animatorPose;
	private float      coefForward;
	private float      coefRight;
	private Vector3    newPoint;
	private bool       useNewPoint;

	#endregion

	public Vector3 NewPoint{
		set{
			newPoint    = value;
			useNewPoint = true;
			coefForward = 0;
			coefRight   = 0;
		}
	}

	public bool UseAnimationOnCancel = false;

	void Awake () 
	{
		Grid         = GlobalOptions.Grid;
		animator     = GetComponent<Animator>();
		animatorPose = GameObject.Find("PlayerModel_imporded").GetComponent<Animator>();
	}

	public bool isMoving(){
		return !animator.GetCurrentAnimatorStateInfo(0).IsName("Empty"); 	
	}

	public bool MovingKeyDown(KeyCode key, bool bakehistory = true){

		if(isMoving()) return false;

		Vector3 direction = Vector3.zero;

		switch (key){
		case KeyCode.UpArrow:
			direction = transform.forward;
			animator.SetTrigger("UpArrow");
			animatorPose.SetTrigger("Forward");
			if(bakehistory)
				ActionHistory.ActionHistoryManager.AddToHistory(ActionHistoryType.Forward, this.gameObject);
		break;
		case KeyCode.DownArrow:
			direction = -transform.forward;
			animator.SetTrigger("DownArrow");
			animatorPose.SetTrigger("Back");
			if(bakehistory)
				ActionHistory.ActionHistoryManager.AddToHistory(ActionHistoryType.Back, this.gameObject);
		break;
		case KeyCode.LeftArrow:
			direction = -transform.right;
			animator.SetTrigger("LeftArrow");
			animatorPose.SetTrigger("Left");
			if(bakehistory)
				ActionHistory.ActionHistoryManager.AddToHistory(ActionHistoryType.Left, this.gameObject);
		break;
		case KeyCode.RightArrow:
			direction = transform.right;
			animator.SetTrigger("RightArrow");
			animatorPose.SetTrigger("Right");
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
			coefForward = -90*Vector3.Dot(direction,transform.forward);
			coefRight   = 90*Vector3.Dot(direction,transform.right);
			newPoint    = Vector3.zero;
			useNewPoint = false;

			//if(coefForward != 0)
			//	transform.Rotate(transform.right, coefForward, Space.World);
			
			//if(coefRight != 0)
			//	transform.Rotate(transform.forward, coefRight, Space.World);

			if(bakehistory || UseAnimationOnCancel){
				animator.SetTrigger("MoveUp");
				animatorPose.SetTrigger("Move");
			}

		}
		else if(Physics.Raycast(GlobalOptions.CurrentBox.transform.position, direction, out hit, Grid.gridSize, layerMask)){

			newPoint = transform.position + direction*Grid.gridSize; coefForward = 0; coefRight   = 0; useNewPoint = true;

			if(bakehistory || UseAnimationOnCancel){
				animator.SetTrigger("Move");
				animatorPose.SetTrigger("Move");
			}

		}
		else{
			newPoint = new Vector3(transform.position.x + Grid.gridSize*(-transform.up).x + Grid.gridSize*direction.x,
			                               transform.position.y + Grid.gridSize*(-transform.up).y + Grid.gridSize*direction.y,
			                               transform.position.z + Grid.gridSize*(-transform.up).z + Grid.gridSize*direction.z);
			
			coefForward = 90*Vector3.Dot(direction,transform.forward);
			coefRight   = -90*Vector3.Dot(direction,transform.right);
			useNewPoint = true;

			if(bakehistory || UseAnimationOnCancel){
				animator.SetTrigger("MoveDown");
				animatorPose.SetTrigger("Move");
			}
		}

		animator.Update(Time.deltaTime); animatorPose.Update(Time.deltaTime);

		return true;
	}	

	public void DragMainPivot(){

		if(useNewPoint)
			transform.position = newPoint;

		if(coefForward != 0)
			transform.Rotate(transform.right, coefForward, Space.World);
		
		if(coefRight != 0)
			transform.Rotate(transform.forward, coefRight, Space.World);
	}
}
