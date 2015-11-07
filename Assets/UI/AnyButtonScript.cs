using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AnyButtonScript : MonoBehaviour {

		// Добавил коммент
		public virtual void AwakeBase(){
			Button buttonComponent = GetComponentInChildren<Button>();
			if (buttonComponent == null){
				return;
			}
			
			buttonComponent.onClick.AddListener(onClick);
		}
		
		public virtual void onClick(){
			Debug.Log("Its a parent method");
		}
	
}
