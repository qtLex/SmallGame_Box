using UnityEngine;
using System;
using System.Collections;
using Enums;
using GameEnums;

namespace BoxClasses
{
	public class BaseBox: MonoBehaviour
	{
		protected Animator thisAnimator;

		public virtual void StartGame(){}

		public virtual void UserAction(object sender, EventArgs evArgs){}

		public virtual void UserStay(){}

		// + mors

		public virtual void OnApproach(){

		}

		public virtual void OnRetire(){
			
		}

		public virtual void OnSpawn(){
			//thisAnimator.SetTrigger("Spawn");
		}

		void OnEnable(){

			thisAnimator = GetComponent<Animator>();
			OnSpawn();
		}
		// - mors
	}

	public class BaseCancelAction: MonoBehaviour{
		
		public virtual bool CancelAction(ActionHistoryType type){return true;}
		
	}
}
