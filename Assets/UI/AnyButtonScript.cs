using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AnyButtonScript : MonoBehaviour {
	
		public virtual void AwakeBase(){
			Button buttonComponent = GetComponent<Button>();
			if (buttonComponent == null){
				return;
			}
			
			buttonComponent.onClick.AddListener(onClick);
		}
		
		public virtual void onClick(){
			Debug.Log("Its a parent method");
		}
	
}
