// ==================================================
// @Author: caiguorong
// @Date: 
// @Desc: 
// ==================================================

using UnityEngine;

public class SingletonMono<T> : MonoBehaviour, ISingleton where T : MonoBehaviour, ISingleton, new()
{
	#region Private Fields
	private static T _ins;
	private bool _isDestroyed;
	private bool _isAppQuitting;
	#endregion

	#region Public Properties
	public static T Ins
	{
		get
		{
			if (_ins == null)
			{
				_ins =  GetOrCreateIns();
			}
			return _ins;
		}
	}
	
	public static bool IsValid
	{
		get { return _ins != null; }
	}
	#endregion

	#region Create
	private static T GetOrCreateIns()
	{
		if (_ins == null)
		{
			_ins = (T) FindObjectOfType(typeof(T));
			if (_ins == null)
			{
				var obj = new GameObject(typeof(T).Name);
				_ins = obj.AddComponent<T>();
			}
		}
		return _ins;
	}
	#endregion

	#region Init Methods

	private void Awake()
	{
		if (_ins == null)
		{
			_ins = this as T;
		}
		// SingletonManager.AddInstance(_ins);
		if (Application.isPlaying)
		{
			DontDestroyOnLoad(transform.root);
		}
		OnInit();
	}

	protected virtual void OnInit()
	{
		
	}
	#endregion

	#region Dispose
	public void OnDestroy()
	{
		OnDispose();
		// SingletonManager.RemoveInstance(_ins);
		_ins = null;
	}
	
	// private void OnApplicationQuit()
	// {
	// 	_isAppQuitting = true;
	// }
	
	protected virtual void OnDispose()
	{
		
	}
	#endregion
}