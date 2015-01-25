using UnityEngine;
using System.Collections;
using BoxClasses;
using GameEnums;

public class BoxController : MonoBehaviour
{
	public BaseBox Box;

	// + mors
	void OnEnable(){
		EditEvent(UserActions.Spawn);
	}
	// - mors

	public void EditEvent(UserActions EventName)
	{
		if(!Box)
			return;


		switch (EventName)
		{
			case UserActions.Action:
//				Box.UserAction();
				break;

			case UserActions.Come:
				Box.UserStay();
				break;

			// + mors
			case UserActions.Spawn:
				Box.OnSpawn();
				break;
			// - mors

			default:
				return;
		}
	}
}
