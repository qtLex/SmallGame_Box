using UnityEngine;
using System.Collections;
using BoxClasses;

public class JumpBox : BaseBox {

	public GameObject target;

	public override void UserStay(){

		if(!target)
			return;

		Vector3 targetPoint = target.transform.position;
		transform.position.Set(targetPoint.x,
		                       targetPoint.y,
		                       targetPoint.z);

	}
}
