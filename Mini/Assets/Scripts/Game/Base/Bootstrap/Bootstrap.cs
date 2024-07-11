// ==================================================
// @Author: caiguorong
// @Date: 
// @Desc: 
// ==================================================

using UnityEngine;

public class Bootstrap : MonoBehaviour
{
	#region Mono Methods

	private void Start()
	{
		SceneMgr.Ins.ChangeScene(EnumSceneType.Merge);
	}

	#endregion
}