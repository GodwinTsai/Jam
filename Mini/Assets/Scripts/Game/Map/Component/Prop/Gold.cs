// ==================================================
// @Author: caiguorong
// @Maintainer: 
// @Date: 
// @Desc: 
// ==================================================
using UnityEngine;

public class Gold : MonoBehaviour
{
	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			GameFlowController.Ins.audioMgr.PlayEatZan();
			EffectMgr.Ins.PlayEatEffect(transform.position);
			DataCenter.Ins.AddThumbUp();
			Destroy(gameObject);
		}
	}
}