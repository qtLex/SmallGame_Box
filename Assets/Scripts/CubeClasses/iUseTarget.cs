using UnityEngine;
using System.Collections;

public interface iUseTarget
{
	Vector3 GetTargetPosition();

	void SetTarget(GameObject Target);
}
