// ==================================================
// @Author: caiguorong
// @Maintainer: 
// @Date: 
// @Desc: 
// ==================================================
using UnityEngine;

public class AutoScroll : MonoBehaviour
{
	#region Mono Fields
	public SpriteRenderer spriteRenderer;
	public float speed = 1;
	#endregion

	private float _tickTime;

	private void Awake()
	{
		if (spriteRenderer == null)
		{
			spriteRenderer = GetComponent<SpriteRenderer>();
		}
	}

	private void Update()
	{
		if (spriteRenderer == null)
		{
			return;
		}

		var player = GameMgr.Ins.player;
		if (player == null)
		{
			return;
		}
		
		var playerSpeed = player.Velocity.x;
		spriteRenderer.material.mainTextureOffset += new Vector2(playerSpeed * speed * Time.deltaTime, 0);
	}
	
}