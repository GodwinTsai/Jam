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
			GameFlowController.Ins.audioMgr.PlayEatBadZan();
			EffectMgr.Ins.PlayEatBadEffect(transform.position);
			DataCenter.Ins.MinusLove();
			Destroy(gameObject);
		}
	}
}