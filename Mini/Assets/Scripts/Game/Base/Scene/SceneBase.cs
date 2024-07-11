// ==================================================
// @Author: caiguorong
// @Date: 
// @Desc: 
// ==================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class SceneBase
{
	#region Private Fields

	protected SceneMgr sceneMgr = SceneMgr.Ins;
	protected delegate IEnumerator LoadSceneStepDelegate();
	#endregion

	#region Public Properties
	public abstract EnumSceneType SceneType { get; }
	#endregion

	#region Enter

	public void Enter()
	{
		//show Loading
		ChangeScene();
	}
	
	#endregion

	#region Exis
	public void Exit()
	{
		OnExit();
	}
	
	#endregion

	#region Load Scene

	private void ChangeScene()
	{
		sceneMgr.StartCoroutine(LoadSceneSteps());
	}

	private IEnumerator LoadSceneSteps()
	{
		//show Loading
		var steps = GetLoadSceneSteps();
		foreach (var step in steps)
		{
			yield return step.Invoke();
		}
		//Hide Loading
		OnEnter();
	}

	protected virtual List<LoadSceneStepDelegate> GetLoadSceneSteps()
	{
		var list = new List<LoadSceneStepDelegate>();
		list.Add(GarbageCollect);
		list.Add(LoadSceneAsync);
		list.Add(InitScene);
		return list;
	}

	protected virtual IEnumerator GarbageCollect()
	{
		UnloadLastSceneAssets();
		yield return null;
		//Unload Unused Assets
		//abm count reference
		yield return Resources.UnloadUnusedAssets();
		//GC
		System.GC.Collect();
		System.GC.WaitForPendingFinalizers();
		yield return null;
	}

	private IEnumerator LoadSceneAsync()
	{
		var sceneName = SceneType.ToString();
		// var async = SceneManager.LoadSceneAsync(sceneName);
		var async = ResMgr.Ins.LoadSceneAsync(sceneName);
		var beginProgress = 0f;
		var endProgress = 1f;
		var progress = 0f;
		while (!async.IsDone)
		{
			progress = Mathf.Lerp(beginProgress, endProgress, async.PercentComplete);
			yield return null;
		}

		OnLoadSceneFinished();
	}

	private IEnumerator InitScene()
	{
		yield return OnInitScene();
	}
	
	#endregion

	#region Virtual Methods
	
	protected virtual void UnloadLastSceneAssets()
	{
		
	}

	protected virtual void OnLoadSceneFinished()
	{
		
	}
	
	protected virtual IEnumerator OnInitScene()
	{
		yield return null;
	}
	
	protected virtual void OnHideLoadingView()
	{
		
	}
	
	protected virtual void OnEnter()
	{
		
	}
	
	protected virtual void OnExit()
	{
		
	}
	#endregion
}