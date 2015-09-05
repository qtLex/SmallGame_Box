﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameEnums;
using BoxClasses;

namespace ActionHistory{
	static public class ActionHistoryManager{
		static List<UserActionItem> ActionList = new List<UserActionItem>();

		public static void AddToHistory(ActionHistoryType type, GameObject obj, Vector3 direction){
			lock(ActionList){
				ActionList.Add(new UserActionItem(type, obj, direction));
			}
		}

		public static void CancelLastAction(){
			lock(ActionList){
				if(ActionList.Count > 0){
					UserActionItem item = ActionList[ActionList.Count-1];

					BaseCancelAction cancelacton = item.actionobject.GetComponent<BaseCancelAction>();

					if(!cancelacton || cancelacton.CancelAction(item.type, item.direction))
						ActionList.Remove(item);
				}
			}
		}
	}

	public class UserActionItem
    {
		public ActionHistoryType type;
		public GameObject actionobject;
        public Vector3 direction;

		public UserActionItem(ActionHistoryType movetype, GameObject actobj, Vector3 actdirection){
            type = movetype; actionobject = actobj; direction = actdirection;
		}
	}
}