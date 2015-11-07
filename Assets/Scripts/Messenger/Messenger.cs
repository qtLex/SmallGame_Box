using System;
using System.Collections.Generic;
using MessengerLib;

// без параметров
static public class Messenger
{
    static public void AddListener(string eventName, EventHandler handler, float timeout = 0)
	{
        MessengerList.Add(eventName, handler, timeout);
	}

    static public void AddListener<TEventArgs>(string eventName, EventHandler<TEventArgs> handler, float timeout = 0) where TEventArgs:EventArgs
    {
        MessengerList<TEventArgs>.Add(eventName, handler, timeout);
    }
	
	static public void RemoveListener(string eventName, EventHandler handler, float timeout = 0)
	{
        MessengerList.Remove(eventName, handler, timeout);
	}

    static public void RemoveListener<TEventArgs>(string eventName, EventHandler<TEventArgs> handler, float timeout = 0) where TEventArgs:EventArgs
    {
        MessengerList<TEventArgs>.Remove(eventName, handler, timeout);
    }
    	
	static public void Invoke(string eventName, object sender)
	{
        EventDescription[] items = MessengerList.GetElements(eventName);
		if(items.Length > 0)
        	foreach(EventDescription item in items)
				item.Execute(sender, EventArgs.Empty);	
	}

    static public void Invoke<TEventArgs>(string eventName, object sender, TEventArgs eventargs) where TEventArgs:EventArgs
	{

        EventDescription<TEventArgs>[] items = MessengerList<TEventArgs>.GetElements(eventName);
		if(items.Length > 0){
            foreach (EventDescription<TEventArgs> item in items)
				item.Execute(sender, eventargs);
		}
	}
}

static public class MessengerList
{
    private static string EVENT_NAME_TO_FIND;
    private static float TIME_OUT_TO_FIND;
    private static List<EventDescription> EventDescriptionList = new List<EventDescription>();

    static private void SetFindValue(string eventName, float timeout)
    {
        EVENT_NAME_TO_FIND = eventName;
        TIME_OUT_TO_FIND = timeout;
    }

    static private bool FindEventDescriptionForName(EventDescription item)
    {
        return item.IsName(EVENT_NAME_TO_FIND);
    }

    static private bool FindEventDescriptionForNameAntTimeOut(EventDescription item)
    {
        return item.IsName(EVENT_NAME_TO_FIND, TIME_OUT_TO_FIND);
    }

    static public EventDescription[] GetElements(string name) 
    {
        SetFindValue(name, 0f);

        return EventDescriptionList.FindAll(FindEventDescriptionForName).ToArray();

    }

    static public void Add(string eventName, EventHandler handler, float timeout = 0)
    {
        lock (EventDescriptionList)
        {
            EventDescription item = EventDescriptionList.Find(FindEventDescriptionForNameAntTimeOut);

            if (item == null)
                EventDescriptionList.Add(new EventDescription(eventName, handler, timeout));
            else
                item.handler += handler;
        }
    }

    static public void Remove(string eventName, EventHandler handler, float timeout = 0)
    {
        lock (EventDescriptionList)
        {
            EventDescription item = EventDescriptionList.Find(FindEventDescriptionForNameAntTimeOut);

            if (item != null)
            {
                item.handler -= handler;

                if (item.Empty()) { EventDescriptionList.Remove(item); }
            }
        }
    }
}

static public class MessengerList<TEventArgs> where TEventArgs:EventArgs
{
    private static string EVENT_NAME_TO_FIND;
    private static float TIME_OUT_TO_FIND;
    private static List<EventDescription<TEventArgs>> EventDescriptionList = new List<EventDescription<TEventArgs>>();

    static private void SetFindValue(string eventName, float timeout)
    {
        EVENT_NAME_TO_FIND = eventName;
        TIME_OUT_TO_FIND = timeout;
    }

    static private bool FindEventDescriptionForName(EventDescription<TEventArgs> item)
    {
        return item.IsName(EVENT_NAME_TO_FIND);
    }

    static private bool FindEventDescriptionForNameAntTimeOut(EventDescription<TEventArgs> item)
    {
        return item.IsName(EVENT_NAME_TO_FIND, TIME_OUT_TO_FIND);
    }

    static public EventDescription<TEventArgs>[] GetElements(string name)
    {
        SetFindValue(name, 0f);

        return EventDescriptionList.FindAll(FindEventDescriptionForName).ToArray();

    }

    static public void Add(string eventName, EventHandler<TEventArgs> handler, float timeout = 0)
    {
        lock (EventDescriptionList)
        {
            EventDescription<TEventArgs> item = EventDescriptionList.Find(FindEventDescriptionForNameAntTimeOut);

            if (item == null)
                EventDescriptionList.Add(new EventDescription<TEventArgs>(eventName, handler, timeout));
            else
                item.handler += handler;
        }
    }

    static public void Remove(string eventName, EventHandler<TEventArgs> handler, float timeout = 0)
    {
        lock (EventDescriptionList)
        {
            EventDescription<TEventArgs> item = EventDescriptionList.Find(FindEventDescriptionForNameAntTimeOut);

            if (item != null)
            {
                item.handler -= handler;

                if (item.Empty()) { EventDescriptionList.Remove(item); }
            }
        }
    }
}