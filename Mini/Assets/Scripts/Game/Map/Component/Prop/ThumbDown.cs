// ==================================================
// @Author: caiguorong
// @Maintainer: 
// @Date: 
// @Desc: 
// ==================================================
using UnityEngine;

public class ThumbDown : MonoBehaviour
{
	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			GameFlowController.Ins.audioMgr.PlayEatBadZan();
			EffectMgr.Ins.PlayEatBadEffect(transform.position);
			ThumbUpMgr.Ins.AddThumbDown(15);
			Destroy(gameObject);
		}
	}
}