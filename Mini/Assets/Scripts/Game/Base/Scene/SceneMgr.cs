// ==================================================
// @Author: caiguorong
// @Date: 
// @Desc: 
// ==================================================

public class SceneMgr : SingletonMono<SceneMgr>
{
	#region Private Fields

	private SceneFactory _factory;
	private SceneBase _currentScene;

	private EnumSceneType _lastSceneType = EnumSceneType.Launch;
	private EnumSceneType _curSceneType = EnumSceneType.Launch;

	#endregion

	#region Public Properties
	public EnumSceneType LastSceneType
	{
		get { return _lastSceneType; }
	}

	public EnumSceneType CurSceneType
	{
		get { return _curSceneType; }
		set
		{
			_lastSceneType = _curSceneType;
			_curSceneType = value; 
		}
	}
	#endregion

	#region Init Methods

	protected override void OnInit()
	{
		_factory = new SceneFactory();
	}

	#endregion

	#region Public Methods
	public void ChangeScene(EnumSceneType sceneType)
	{
		_currentScene?.Exit();
		_currentScene = _factory.CreateScene(sceneType);
		_currentScene.Enter();
		CurSceneType = _currentScene.SceneType;
	}
	
	#endregion

}