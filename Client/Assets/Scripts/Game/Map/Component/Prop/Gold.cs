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
			GameFlowController.Instance.audioMgr.PlayEatZan();
			EffectMgr.Instance.PlayEatEffect(transform.position);
			DataCenter.Instance.AddThumbUp();
			Destroy(gameObject);
		}
	}
}