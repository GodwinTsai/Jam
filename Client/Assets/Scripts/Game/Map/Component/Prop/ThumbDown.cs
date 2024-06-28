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
			GameFlowController.Instance.audioMgr.PlayEatBadZan();
			EffectMgr.Instance.PlayEatBadEffect(transform.position);
			ThumbUpMgr.Instance.AddThumbDown(15);
			Destroy(gameObject);
		}
	}
}