using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class DeferredExecution : MonoBehaviour {

	public List<DeferredEvent> EventList = new List<DeferredEvent>();

	public class  DeferredEvent{
		private EventHandler _handler;
		private float _timeout;
		private EventArgs _args;
		private object _sender;

		public EventHandler handler{
			get
			{
				return _handler;
			}
		}

		public float timeout{
			get
			{ 
				return _timeout;
			}
			set
			{
				_timeout -= value;
				if(_timeout < 0)
					_timeout = 0;
			}
		}

		public EventArgs args{
			get
			{
				return _args;
			}
		} 

		public object sender{
			get
			{
				return _sender;
			}
		}

		public DeferredEvent(object sender,EventHandler handler, float timeout, EventArgs args){
			_sender = sender;
			_handler = handler;
			_timeout = timeout;
			_args = args;
		}
	}

	public void AddEvent(object sender,EventHandler handler, float timeout, EventArgs args)
	{
		EventList.Add(new DeferredEvent(sender, handler, timeout, args));
	}

	void Update () {

		float dtime = Time.deltaTime;

		foreach(DeferredEvent dEvent in EventList.ToArray()){
			dEvent.timeout = dtime;

			if(dEvent.timeout == 0){
				dEvent.handler(dEvent.sender, dEvent.args);
				EventList.Remove(dEvent);
			}
		}
	}
}
