// ==================================================
// @Author: caiguorong
// @Date: 
// @Desc: 
// ==================================================

using System;

public class SingletonClass<T> : ISingleton where T : SingletonClass<T>, new()
{
	#region Private Fields
	private static T _ins;
	#endregion

	#region Public Properties
	public static T Ins
	{
		get
		{
			if (_ins == null)
			{
				_ins =  Activator.CreateInstance<T>();
				_ins.Init();
			}
			return _ins;
		}
	}
	
	public static bool IsValid
	{
		get { return _ins != null; }
	}
	#endregion

	#region Init

	public void Init()
	{
		OnInit();
	}

	protected virtual void OnInit()
	{
		
	}
	#endregion

	#region Dispose

	public void Dispose()
	{
		OnDispose();
		_ins = null;
	}
	
	protected virtual void OnDispose()
	{
		
	}
	#endregion
}