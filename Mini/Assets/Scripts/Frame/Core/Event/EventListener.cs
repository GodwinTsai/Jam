// ==================================================
// @Author: caiguorong
// @Date: 
// @Desc: 
// ==================================================

using System;

public interface IEventListener
{
	System.Type EventType { get; }
	public void MarkDied();
}

public class EventListener<TEvent> : IEventListener where TEvent : struct, IEvent
{
	#region Private Fields
	public delegate void EventDelegate(TEvent e);
	
	private bool _isDied;
	private int _countLimit;
	
	private EventDelegate _callback;
	#endregion

	#region Public Properties

	public System.Type EventType
	{
		get { return typeof(TEvent); }
	}

	public EventDelegate Callback
	{
		get { return _callback; }
	}

	public bool IsDied
	{
		get { return _isDied; }
	}

	#endregion

	#region Public Methods
	public EventListener(EventDelegate callback, int countLimit)
	{
		_callback = callback;
		_countLimit = countLimit;
	}
	
	public void MarkDied()
	{
		_isDied = true;
		_callback = null;
	}
	
	public void InvokeCallback(TEvent e)
	{
		_callback.Invoke(e);
			
		if (_countLimit > 0)
		{
			_countLimit--;
			if (_countLimit == 0) 
			{
				MarkDied();
			}
		}
	}
	#endregion

}