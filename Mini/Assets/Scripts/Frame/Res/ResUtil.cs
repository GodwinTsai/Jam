// ==================================================
// @Author: caiguorong
// @Date: 
// @Desc: 
// ==================================================

using UnityEngine;

public class ResUtil
{
	#region Mono Fields

	#endregion

	#region Private Fields

	private IResMgr _resMgr = new ResMgr();

	#endregion

	#region Public Properties

	#endregion

	#region Mono Methods

	#endregion

	#region Public Methods
	public static GameObject LoadPrefab(string path)
	{
		var obj = Resources.Load<GameObject>(path);
		if (obj != null)
		{
			var prefab = GameObject.Instantiate(obj);
			return prefab;
		}

		return null;
	}
	#endregion

	#region Private Methods

	#endregion
}