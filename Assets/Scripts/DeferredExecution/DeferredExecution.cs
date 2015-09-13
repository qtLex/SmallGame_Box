using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class DeferredExecution : MonoBehaviour {

	public List<AbstractDeferredEvent> EventList = new List<AbstractDeferredEvent>();

    public abstract class AbstractDeferredEvent
    {
        private float _timeout;

        public float timeout
        {
            get
            {
                return _timeout;
            }
            set
            {
                _timeout -= value;
                if (_timeout < 0)
                    _timeout = 0;
            }
        }

        public abstract void Execute();
    } 

	public class DeferredEvent:AbstractDeferredEvent
    {
		private EventHandler _handler;
		private EventArgs _args;
		private object _sender;

		public EventHandler handler{
			get
			{
				return _handler;
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

		public DeferredEvent(object sender, EventHandler handler, float timeOut, EventArgs args){
			_sender = sender;
			_handler = handler;
			timeout = timeOut;
			_args = args;
		}

        public override void Execute()
        {
            _handler(_sender, _args);
        }
	}

    public class DeferredEvent<TEventArgs>:AbstractDeferredEvent where TEventArgs : EventArgs
    {
        private EventHandler<TEventArgs> _handler;
        private TEventArgs _args;
        private object _sender;

        public EventHandler<TEventArgs> handler
        {
            get
            {
                return _handler;
            }
        }

        public TEventArgs args
        {
            get
            {
                return _args;
            }
        }

        public object sender
        {
            get
            {
                return _sender;
            }
        }


        public DeferredEvent(object sender, EventHandler<TEventArgs> handler, float timeOut, TEventArgs args)
        {
            _sender = sender;
            _handler = handler;
            timeout = timeOut;
            _args = args;
        }

        public override void Execute()
        {
            _handler(_sender, _args);
        }
    }

	public void AddEvent(object sender, EventHandler handler, float timeout, EventArgs args)
	{
		EventList.Add(new DeferredEvent(sender, handler, timeout, args));
	}

    public void AddEvent<TEventArgs>(object sender, EventHandler<TEventArgs> handler, float timeout, TEventArgs args) where TEventArgs:EventArgs
    {
        EventList.Add(new DeferredEvent<TEventArgs>(sender, handler, timeout, args));
    }

	void Update () {

		float dtime = Time.deltaTime;

		foreach(AbstractDeferredEvent dEvent in EventList.ToArray()){
			dEvent.timeout = dtime;

			if(dEvent.timeout == 0){
				dEvent.Execute();
				EventList.Remove(dEvent);
			}
		}
	}
}
