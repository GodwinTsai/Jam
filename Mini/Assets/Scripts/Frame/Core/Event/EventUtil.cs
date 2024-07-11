// ==================================================
// @Author: caiguorong
// @Date: 
// @Desc: 
// ==================================================

using System;
using System.Collections.Generic;

public static class EventUtil
{
	#region Private Fields

	private static Dictionary<Type, IEventDispatcher> _dispatchers = new();

	#endregion

	#region Add Listener
	public static EventListener<T> AddListener<T>(EventListener<T>.EventDelegate callback) where T : struct, IEvent
	{
		return AddListener(callback, -1);
	}

	public static EventListener<T> AddListenerOnce<T>(EventListener<T>.EventDelegate callback) where T : struct, IEvent
	{
		return AddListener(callback, 1);
	}
	
	private static EventListener<T> AddListener<T>(EventListener<T>.EventDelegate callback, int countLimit) where T : struct, IEvent
	{
		if (callback == null)
		{
			return null;
		}

		var listener = new EventListener<T>(callback, countLimit);//buffer

		var dispatcher = GetDispatcher<T>();
		dispatcher.AddListener(listener);

		return listener;
	}
	
	#endregion

	#region Remove Listener
	public static void RemoveListener(IEventListener listener)
	{
		if (listener == null)
		{
			return;
		}
		var dispatcher = FindDispatcher(listener.EventType);
		if (dispatcher == null)
		{
			return;
		}
		dispatcher.RemoveListener(listener);
	}
	
	public static void RemoveListener<T>(EventListener<T>.EventDelegate callback) where T : struct, IEvent
	{
		if (callback == null)
		{
			return;
		}
		var dispatcher = FindDispatcher<T>();
		if (dispatcher == null)
		{
			return;
		}
		dispatcher.RemoveListener(callback);
	}

	#endregion

	#region Get Dispatch
	private static EventDispatcher<T> GetDispatcher<T>() where T : struct, IEvent
	{
		Type type = typeof(T);
		var dispatcher = FindDispatcher<T>();
		if (dispatcher == null)
		{
			dispatcher = new EventDispatcher<T>();//buffer
			_dispatchers.Add(type, dispatcher);
		}

		return dispatcher;
	}

	private static EventDispatcher<T> FindDispatcher<T>() where T : struct, IEvent
	{
		var type = typeof(T);
		return FindDispatcher(type) as EventDispatcher<T>;
	}
	
	private static IEventDispatcher FindDispatcher(Type type)
	{
		return _dispatchers.GetValueOrDefault(type);
	}
	#endregion

	#region Dispatch
	public static void Dispatch<T>(T evt) where T : struct, IEvent
	{
		var dispatcher = FindDispatcher<T>();
		dispatcher?.Dispatch(evt);
	}
	#endregion
}