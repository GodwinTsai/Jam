// ==================================================
// @Author: caiguorong
// @Maintainer: 
// @Date: 
// @Desc: 
// ==================================================
using UnityEngine;

public class Kick : MonoBehaviour
{
	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			GameFlowController.Ins.audioMgr.PlayEatZan();
			EffectMgr.Ins.PlayEatEffect(transform.position);
			EffectMgr.Ins.PlayKickBrokenEffect(transform.position);
			ThumbUpMgr.Ins.AddThumbUp(5);
			Destroy(gameObject);
		}
	}
}