// ==================================================
// @Author: caiguorong
// @Maintainer: 
// @Date: 
// @Desc: 
// ==================================================
using UnityEngine;

public class AddLove : MonoBehaviour
{
	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			GameFlowController.Ins.audioMgr.PlayEatZan();
			EffectMgr.Ins.PlayEatEffect(transform.position);
			ThumbUpMgr.Ins.AddLove(3);
			Destroy(gameObject);
		}
	}

}