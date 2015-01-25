using UnityEngine;
using System.Collections;
using GameEnums;


public class PlayerControllerBack : MonoBehaviour {

	
	#region Private
	private CubeGrid   Grid;
	private GameObject CurrentBox;
	private bool moving = false;
	#endregion

	// Use this for initialization
	void Awake () 
	{
		Grid = GlobalOptions.Grid;
	}
	
	// Update is called once per frame
	void Update ()
	{
		CurrentBox = GlobalOptions.CurrentBox;

		if(moving || Time.timeScale == 0)
			return;

		#region GetDirection
		Vector3 direction = Vector3.zero;
		if(Input.GetKeyDown(KeyCode.Space))
		{
			// тут вызываем действие
			if(!CurrentBox)
				return;

			CurrentBox.GetComponent<BoxController>().EditEvent(UserActions.Action);
		}
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            direction = transform.forward;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            direction = -transform.forward;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            direction = transform.right;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            direction = -transform.right;
        }

		#endregion

		if(direction == Vector3.zero)
		{
			return;
		}
		else
		{
			moving = true;
		}

		// определим нужно ли подниматься на куб
		RaycastHit hit = new RaycastHit();

		int layerMask = 1 << LayerMask.NameToLayer("Box");

		// выполним поворот объекта в направлении движения
		//transform.rotation = Quaternion.FromToRotation(transform.forward, direction);

		if(Physics.Raycast(transform.position, direction, out hit, Grid.gridSize, layerMask))
		{
			float coef = (Grid.gridSize/2)-this.transform.localScale.y/2;
			Vector3 newPoint = new Vector3(transform.position.x + coef*transform.up.x + coef*direction.x,
			                               transform.position.y + coef*transform.up.y + coef*direction.y,
			                               transform.position.z + coef*transform.up.z + coef*direction.z);

		   	transform.position = newPoint;

			// выполним поворот

			float coefForward = -90*Vector3.Dot(direction,transform.forward);
			float coefRight   = 90*Vector3.Dot(direction,transform.right);

			if(coefForward != 0)
				transform.Rotate(transform.right, coefForward, Space.World);

			if(coefRight != 0)
				transform.Rotate(transform.forward, coefRight, Space.World);

			//transform.up = -direction;
		}
		else if(Physics.Raycast(CurrentBox.transform.position, direction, out hit, Grid.gridSize, layerMask))
		{
			// переместим на следующий куб по прямой

			Vector3 newPoint = new Vector3(transform.position.x + Grid.gridSize*direction.x,
			                               transform.position.y + Grid.gridSize*direction.y,
			                               transform.position.z + Grid.gridSize*direction.z);

			transform.position = newPoint;
		}
		else
		{
			float coef = (Grid.gridSize/2)+this.transform.localScale.y/2;
			Vector3 newPoint = new Vector3(transform.position.x + coef*(-transform.up).x + coef*direction.x,
			                               transform.position.y + coef*(-transform.up).y + coef*direction.y,
			                               transform.position.z + coef*(-transform.up).z + coef*direction.z);

			transform.position = newPoint;

			float coefForward = 90*Vector3.Dot(direction,transform.forward);
			float coefRight   = -90*Vector3.Dot(direction,transform.right);

			if(coefForward != 0)
				transform.Rotate(transform.right, coefForward, Space.World);

			if(coefRight != 0)
				transform.Rotate(transform.forward, coefRight, Space.World);
		}

		moving = false;
	}
}
