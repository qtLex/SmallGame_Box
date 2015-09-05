using UnityEngine;
using System;
using System.Collections;
using BoxClasses;

public class JumpBox : BaseBox, iUseTarget
{

	public GameObject target;

	private bool _initialized;

	public Vector3 GetTargetPosition()
	{
		return !target ? Vector3.zero : target.transform.position;
	}

	public void Start(){

		if (_initialized){return;};

		_initialized = true;

	}

	public void SetTarget(GameObject Target)
	{
		target = Target;
		Start();
	}

	public override void UserStay()
	{
		if(!target){
			return;
		}

		Debug.Log(target.transform.position.ToString() + " " + transform.position.ToString());

		Vector3 targetPoint = target.transform.position;
		GlobalOptions.Player.transform.position = targetPoint;

		GlobalOptions.Player.transform.up      = target.transform.up;
		GlobalOptions.Player.transform.forward = target.transform.forward;
		GlobalOptions.Player.transform.right   = target.transform.right;

	}
	
}
