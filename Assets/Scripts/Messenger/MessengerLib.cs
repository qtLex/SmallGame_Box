using System;

namespace MessengerLib
{
    public class EventDescription
    {

        private string eventName;
        private float timeOut;
        private EventHandler _handler;

        public EventDescription(string eventname, EventHandler handler, float timeout = 0f)
        {
            eventName = eventname;
            timeOut = timeout;
            _handler += handler;
        }

        public event EventHandler handler
        {
            add { _handler += value; }
            remove { _handler -= value; }
        }

        public void Execute(object sender, EventArgs args)
        {
            if (timeOut > 0)
                GlobalOptions.DeferredExecutionComponent.AddEvent(sender, _handler, timeOut, args);
            else
                _handler(sender, args);
        }

        public bool IsName(string eventname)
        {

            return (eventname == eventName);

        }

        public bool IsName(string eventname, float timeout)
        {

            return ((eventname == eventName) & (timeOut == timeout));

        }

        public bool Empty()
        {
            return (_handler == null);
        }
    }
}
