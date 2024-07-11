// ==================================================
// @Author: caiguorong
// @Date: 
// @Desc: 
// ==================================================

using UnityEngine;

public class SingletonMgr : MonoBehaviour
{
	#region Private Fields
	private static SingletonMgr _ins;
	private static bool _isAppQuitting;
	#endregion

	#region Public Properties

	public static SingletonMgr Ins
	{
		get
		{
			if (_isAppQuitting)
			{
				return null;
			}

			if (_ins == null)
			{
				var type = typeof(SingletonMgr);
				_ins = FindObjectOfType(type) as SingletonMgr;
				if (_ins == null)
				{
					var obj = new GameObject(type.Name);
					_ins = obj.AddComponent<SingletonMgr>();
				}
			}

			return _ins;
		}
	}
	#endregion

	#region Mono Methods
	private void Awake()
	{
		if (_ins == null)
		{
			_ins = this;
		}
		if (Application.isPlaying)
		{
			DontDestroyOnLoad(transform.root);
		}
	}
	#endregion

}