using System;
using System.Collections.Generic;
using MessengerLib;

// без параметров
static public class Messenger
{
	private static List<EventDescription> EventDescriptionList = new List<EventDescription>();

	private static string EVENT_NAME_TO_FIND;
	private static float TIME_OUT_TO_FIND;
	
	static public void AddListener(string eventName, EventHandler handler, float timeout = 0)
	{
		SetFindValue(eventName, timeout);

		lock(EventDescriptionList){
			EventDescription item = EventDescriptionList.Find(FindEventDescriptionForNameAntTimeOut);
				
			if(item == null)
				EventDescriptionList.Add(new EventDescription(eventName, handler, timeout));
			else
				item.handler += handler;		
		}
	}
	
	static public void RemoveListener(string eventName, EventHandler handler, float timeout = 0)
	{
		SetFindValue(eventName, timeout);

		lock(EventDescriptionList){
			EventDescription item = EventDescriptionList.Find(FindEventDescriptionForNameAntTimeOut);
			
			if(item != null){
				item.handler -= handler;

				if(item.Empty()){EventDescriptionList.Remove(item);}
			}
		}
	}

	static private void SetFindValue(string eventName, float timeout){
		EVENT_NAME_TO_FIND = eventName;
		TIME_OUT_TO_FIND = timeout;
	}
	
	static public void Invoke(string eventName, object sender)
	{
		SetFindValue(eventName, 0f);

		EventDescription[] items = EventDescriptionList.FindAll(FindEventDescriptionForName).ToArray();
		if(items.Length > 0){
			foreach(EventDescription item in items)
				item.Execute(sender, EventArgs.Empty);
		}
	
	}

	static public void Invoke(string eventName, object sender, EventArgs eventargs)
	{
		SetFindValue(eventName, 0f);
		
		EventDescription[] items = EventDescriptionList.FindAll(FindEventDescriptionForName).ToArray();
		if(items.Length > 0){
			foreach(EventDescription item in items)
				item.Execute(sender, eventargs);
		}
	}

	static private bool FindEventDescriptionForName(EventDescription item){
		return item.IsName(EVENT_NAME_TO_FIND);
	}

	static private bool FindEventDescriptionForNameAntTimeOut(EventDescription item){
		return item.IsName(EVENT_NAME_TO_FIND, TIME_OUT_TO_FIND);
	}
}
