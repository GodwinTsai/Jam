// ==================================================
// @Author: caiguorong
// @Date: 
// @Desc: 
// ==================================================

using System;
using System.Collections.Generic;

internal interface IEventDispatcher
{
	void RemoveListener(IEventListener listener);
}

public class EventDispatcher<TEvent>: IEventDispatcher where TEvent: struct, IEvent
{
	#region Private Fields
	private readonly List<EventListener<TEvent>> _listeners = new();
	
	private int _dispatchingLevel;
	#endregion

	#region Listener
	
	public void AddListener(EventListener<TEvent> listener) 
	{
		_listeners.Add(listener);
	}
	
	public void RemoveListener(IEventListener listener)
	{
		RemoveListener(listener as EventListener<TEvent>);
	}

	public void RemoveListener(EventListener<TEvent> listener) 
	{
		if (listener == null)
		{
			return;
		}
		if (_dispatchingLevel > 0)
		{
			listener.MarkDied();
		}
		else
		{
			_listeners.Remove(listener);
		}
	}

	public void RemoveListener(EventListener<TEvent>.EventDelegate callback)
	{
		var listener = FindListenerByCallback(callback);
		RemoveListener(listener);
	}
	
	private EventListener<TEvent> FindListenerByCallback(EventListener<TEvent>.EventDelegate callback)
	{
		for (int i = 0; i < _listeners.Count; i++)
		{
			if (_listeners[i].Callback == callback)
			{
				return _listeners[i];
			}
		}

		return null;
	}
	#endregion

	#region Dispatch

	public void Dispatch(TEvent evt)
	{
		_dispatchingLevel++;
		
		DispatchEvent(evt);
		
		_dispatchingLevel--;

		RemoveDiedListeners();
	}
	
	private void DispatchEvent(TEvent evt)
	{
		for (int i = _listeners.Count - 1; i >= 0; i--)
		{
			var listener = _listeners[i];
			if (listener.IsDied)
			{
				continue;
			}

			try
			{
				listener.InvokeCallback(evt);
			}
			catch (Exception e)
			{
				LogUtil.LogException(e);
			}
		}
	}
	
	private void RemoveDiedListeners()
	{
		for (int i = _listeners.Count - 1; i >= 0; i--)
		{
			if (_listeners[i].IsDied)
			{
				_listeners.RemoveAt(i);
			}
		}
	}
	#endregion
	
}