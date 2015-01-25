using UnityEngine;
using System.Collections;
using GameEnums;
using BoxClasses;

public class PlayerDetector : MonoBehaviour 
{
	void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.tag != "Player")
			return;

		GlobalOptions.CurrentBox = this.transform.parent.gameObject;
		BaseBox box =  GlobalOptions.CurrentBox.GetComponent<BaseBox>();
		if(box != null){
			box.UserStay();
			Messenger.AddListener("UserAction", box.UserAction);
		}
	}

	void OnTriggerExit(Collider other)
	{
		if(other.gameObject.tag != "Player")
			return;

		BaseBox box =  GlobalOptions.CurrentBox.GetComponent<BaseBox>();
		if(box != null){
			Messenger.RemoveListener("UserAction", box.UserAction);
		}

		GlobalOptions.CurrentBox = null;
	}
}
