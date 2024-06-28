// ==================================================
// @Author: caiguorong
// @Maintainer: 
// @Date: 
// @Desc: 
// ==================================================
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
	void LateUpdate()
	{
		var cam = Camera.main;
		if (cam == null)
		{
			return;
		}
		var pos = cam.transform.position;
		pos.z = 5;
		transform.position = pos;
	}
}