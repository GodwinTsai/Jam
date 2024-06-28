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
			GameFlowController.Instance.audioMgr.PlayEatZan();
			EffectMgr.Instance.PlayEatEffect(transform.position);
			EffectMgr.Instance.PlayKickBrokenEffect(transform.position);
			ThumbUpMgr.Instance.AddThumbUp(5);
			Destroy(gameObject);
		}
	}
}