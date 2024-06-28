// ==================================================
// @Author: caiguorong
// @Maintainer: 
// @Date: 
// @Desc: 
// ==================================================
using UnityEngine;

public class MinusLove : MonoBehaviour
{
	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			GameFlowController.Instance.audioMgr.PlayEatBadZan();
			EffectMgr.Instance.PlayEatBadEffect(transform.position);
			DataCenter.Instance.MinusLove();
			Destroy(gameObject);
		}
	}
}