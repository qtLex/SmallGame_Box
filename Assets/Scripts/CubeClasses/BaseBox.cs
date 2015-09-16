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

        void OnDestroy()
        {
            Messenger.RemoveListener("UserAction", UserAction);
        }

		public virtual bool CanBeConnected(){ return false;}
		public virtual void ConnectTo(GameObject other){}
		public virtual GameObject GetTarget(){return null;}
		public virtual GameObject[] GetTargets(){return null;}

		// - mors
	}

	public class BaseCancelAction: MonoBehaviour{

        public virtual bool CancelAction(ActionHistoryType type, Vector3 lastDirection) { return true; }
		
	}
}
