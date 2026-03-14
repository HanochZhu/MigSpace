using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class EventManager : JigSingleton<EventManager>
{
	private Dictionary<string, EventWithParam> _eventDictionary;

	private static EventManager eventManager;

	public Dictionary<string, List<string>> _allListeners;

	public Dictionary<string, EventWithParam> EventDictionary
	{
		get
		{
			if (_eventDictionary == null)
			{
				_eventDictionary = new Dictionary<string, EventWithParam>();
			}
			return _eventDictionary;
		}
	}

	public Dictionary<string, List<string>> AllListeners
	{
		get
		{
			if (_allListeners == null)
			{
				_allListeners = new Dictionary<string, List<string>>();
			}
			return _allListeners;
		}
	}

	public static void StartListening(string eventName, UnityAction<object, object> listener)
	{
		EventWithParam value = null;
		if (Instance.EventDictionary.TryGetValue(eventName, out value))
		{
			value.AddListener(listener);
		}
		else
		{
			value = new EventWithParam();
			value.AddListener(listener);
			Instance.EventDictionary.Add(eventName, value);
		}
		if (Instance.AllListeners.ContainsKey(eventName))
		{
			Instance.AllListeners[eventName].Add(BuildListenerDetailString(listener));
			return;
		}
		Instance.AllListeners.Add(eventName, new List<string> { BuildListenerDetailString(listener) });
	}

	public static void StopListening(string eventName, UnityAction<object, object> listener)
	{
		if (!(eventManager == null))
		{
			EventWithParam value = null;
			if (Instance.EventDictionary.TryGetValue(eventName, out value))
			{
				value.RemoveListener(listener);
			}
			if (Instance.AllListeners.Keys.Any((string k) => k == eventName) && Instance.AllListeners[eventName].Any((string v) => v == BuildListenerDetailString(listener)))
			{
				Instance.AllListeners[eventName].Remove(BuildListenerDetailString(listener));
			}
		}
	}

	public static void TriggerEvent(string eventName, object parameter1 = null, object parameter2 = null)
	{
		EventWithParam value = null;
		if (!(Instance == null) && Instance.EventDictionary.TryGetValue(eventName, out value))
		{
			value.Invoke(parameter1, parameter2);
		}
	}

	private static string BuildListenerDetailString(UnityAction<object, object> listener)
	{
		return "Script: " + listener.Target.ToString() + " - Listeneer: " + listener.Method.ToString();
	}
}
