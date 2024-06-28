// ==================================================
// @Author: caiguorong
// @Maintainer: 
// @Date: 
// @Desc: 
// ==================================================
using UnityEngine;

public class Hole : MonoBehaviour
{
	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			EventManager.ExecuteEvent(EventConst.EventGameOver);
		}
	}
}