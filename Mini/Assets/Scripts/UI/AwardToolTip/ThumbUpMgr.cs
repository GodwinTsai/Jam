// ==================================================
// @Author: caiguorong
// @Maintainer: 
// @Date: 
// @Desc: 
// ==================================================
using System.Collections;
using UnityEngine;

public class ThumbUpMgr : SingletonMono<ThumbUpMgr>
{
	public void AddLove(int times)
	{
		StartCoroutine(AddLoveCoroutine(times));
	}
	
	public IEnumerator AddLoveCoroutine(int times)
	{
		for (int i = 0; i < times; i++)
		{
			DataCenter.Ins.AddLove();
			var delay = UnityEngine.Random.Range(0, 0.1f);
			yield return new WaitForSeconds(delay);
		}
	}
	
	public void AddThumbUp(int times)
	{
		StartCoroutine(AddThumbUpCoroutine(times));
	}
	
	public IEnumerator AddThumbUpCoroutine(int times)
	{
		for (int i = 0; i < times; i++)
		{
			DataCenter.Ins.AddThumbUp();
			var delay = UnityEngine.Random.Range(0, 0.1f);
			yield return new WaitForSeconds(delay);
		}
	}
	
	public void AddThumbDown(int times)
	{
		StartCoroutine(AddThumbDownCoroutine(times));
	}
	
	public IEnumerator AddThumbDownCoroutine(int times)
	{
		for (int i = 0; i < times; i++)
		{
			DataCenter.Ins.MinusThumbUp();
			var delay = UnityEngine.Random.Range(0, 0.1f);
			yield return new WaitForSeconds(delay);
		}
	}
}