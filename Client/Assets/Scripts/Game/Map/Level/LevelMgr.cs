// ==================================================
// @Author: caiguorong
// @Maintainer: 
// @Date: 
// @Desc: 
// ==================================================
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelMgr : SingletonClassAuto<LevelMgr>
{
	#region Private Fields
	public Scene curLevelScene;
	public int curLevel;

	private Dictionary<int, string> _levelSceneDic;

	#endregion

	#region Public Properties

	public override void Init()
	{
		base.Init();
		if (_levelSceneDic == null)
		{
			_levelSceneDic = new();
			_levelSceneDic.Add(1, "Scenes/Level1");
			_levelSceneDic.Add(2, "Scenes/Level2");
			_levelSceneDic.Add(3, "Scenes/Level3");
		}
	}

	#endregion

	#region Public Methods

	public void ResetLevel()
	{
		curLevel = 0;
	}

	public bool EnterNextLevel()
	{
		var nextLevel = curLevel + 1;
		if (!_levelSceneDic.ContainsKey(nextLevel))
		{
			return false;
		}

		UnloadCurLevel();
		LoadScene(nextLevel);
		return true;
	}

	public void RetryCurLevel()
	{
		UnloadCurLevel();
		LoadScene(curLevel);
	}
	
	private void UnloadCurLevel()
	{
		if (curLevelScene.isLoaded)
		{
			SceneManager.UnloadSceneAsync(curLevelScene);
		}
		
		var player = GameMgr.Instance.player;
		if (player != null)
		{
			GameObject.Destroy(player.gameObject);
		}
	}

	private void LoadScene(int level)
	{
		if (!_levelSceneDic.TryGetValue(level, out var sceneName))
		{
			return;
		}
		var param = new LoadSceneParameters(LoadSceneMode.Additive);
		var scene = SceneManager.LoadScene(sceneName, param);
		curLevelScene = scene;
		
		curLevel = level;
	}
	#endregion

}