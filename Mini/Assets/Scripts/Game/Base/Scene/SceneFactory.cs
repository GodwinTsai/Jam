// ==================================================
// @Author: caiguorong
// @Date: 
// @Desc: 
// ==================================================

using System;
using System.Collections.Generic;

public class SceneFactory
{
	#region Private Fields
	private Dictionary<int, Type> _sceneTypeDic = new();
	#endregion

	#region Register Methods

	public SceneFactory()
	{
		_sceneTypeDic.Clear();
		AddSceneType(EnumSceneType.Merge, typeof(MergeScene));
	}
	
	private void AddSceneType(EnumSceneType sceneType, Type type)
	{
		_sceneTypeDic.Add((int)sceneType, type);
	}
	#endregion

	#region Public Methods

	public SceneBase CreateScene(EnumSceneType sceneType)
	{
		var key = (int)sceneType;
		if (_sceneTypeDic.TryGetValue(key, out var classType))
		{
			return Activator.CreateInstance(classType) as SceneBase;
		}

		return null;
	}
	#endregion

}