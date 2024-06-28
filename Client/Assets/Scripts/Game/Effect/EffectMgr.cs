// ==================================================
// @Author: caiguorong
// @Maintainer: 
// @Date: 
// @Desc: 
// ==================================================
using UnityEngine;

public class EffectMgr : SingletonClassAuto<EffectMgr>
{
	public void PlayEffect(Vector3 position, string effectName)
	{
		GameObject effect = ResouceUtil.LoadPrefab("Effect/" + effectName);
		if (effect == null)
		{
			return;
		}

		position.z = 0;
		effect.transform.position = position;
		
		var effectComponent = effect.GetComponent<Effect>();
		if (effectComponent == null)
		{
			effect.AddComponent<Effect>();
		}
	}

	public void PlayEatEffect(Vector3 position)
	{
		PlayEffect(position, "ChuiZiEffectHuang");
		// PlayEffect(position, "EatProp");
	}
	
	public void PlayEatBadEffect(Vector3 position)
	{
		PlayEffect(position, "ChuiZiEffectHong");
	}
	
	public void PlayKickBrokenEffect(Vector3 position)
	{
		PlayEffect(position, "Broken");
	}
}