// ==================================================
// @Author: caiguorong
// @Maintainer: 
// @Date: 
// @Desc: 
// ==================================================
using UnityEngine;

public class Effect : MonoBehaviour
{
	#region Mono Fields

	public float duration;
	public ParticleSystem particle;

	#endregion

	#region Mono Methods

	private void Awake()
	{
		if (particle == null)
		{
			particle = GetComponent<ParticleSystem>();
		}

		if (particle == null)
		{
			particle = GetComponentInChildren<ParticleSystem>();
		}

		if (duration <= 0)
		{
			if (particle != null)
			{
				duration = particle.main.duration;
			}
			else
			{
				duration = 5;
			}
		}

		Play();
	}

	#endregion

	private void Play()
	{
		if (particle != null)
		{
			particle.Play();
		}
		Invoke("Destroy", duration);
	}

	private void Destroy()
	{
		GameObject.Destroy(gameObject);
	}
}