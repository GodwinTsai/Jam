// ==================================================
// @Author: caiguorong
// @Maintainer: 
// @Date: 
// @Desc: 
// ==================================================
using UnityEngine;

public class ResouceUtil
{
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
}