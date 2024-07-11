// ==================================================
// @Author: caiguorong
// @Maintainer: 
// @Date: 
// @Desc: 
// ==================================================
using System.Collections;
using UnityEngine;

public class MapComponent : MonoBehaviour
{
	#region Mono Fields

	public GameObject startPoint;

	public GameObject moduleRoot;
	public GameObject[] moduleTemplates;

	public bool endlessMode;

	public GameObject playerPrefab;

	#endregion

	#region Private Fields

	private bool _checkLoadModule;
	private GameObject _lastModule;

	#endregion

	#region Public Properties

	public bool CheckLoadModule
	{
		get { return _checkLoadModule; }
		set { _checkLoadModule = value; }
	}

	#endregion

	#region Mono Methods

	private void Awake()
	{
		InitEndlessMode();
		
		StartLoadModule();
		Update();
		StartCoroutine(LoadPlayer());
	}

	private void InitEndlessMode()
	{
		if (_lastModule == null)
		{
			var childCount = moduleRoot.transform.childCount;
			if (childCount > 0)
			{
				var child = moduleRoot.transform.GetChild(childCount - 1);
				_lastModule = child.gameObject;
			}
		}
	}

	private IEnumerator LoadPlayer()
	{
		yield return null;
		GameObject playerObj;
		if (playerPrefab != null)
		{
			playerObj = Instantiate(playerPrefab); 
		}
		else
		{
			playerObj = ResUtil.LoadPrefab("Smile/PlayerPrefab/Player");
		}
		
		var player = playerObj.GetComponent<Player>();
		player.transform.parent = Launch.Current.mapRoot.transform;
		player.gameObject.SetActive(true);
		player.transform.position = startPoint.transform.position;
		GameMgr.Ins.player = player;
		EventManager.ExecuteEvent(EventConst.EventLevelStart, startPoint.transform.position);
	}

	private void Update()
	{
		var needLoad = NeedLoadModule();
		if (needLoad)
		{
			LoadModule();
		}
	}

	#endregion

	#region Load Module

	public void StartLoadModule()
	{
		_checkLoadModule = endlessMode;
	}

	private bool NeedLoadModule()
	{
		if (!endlessMode)
		{
			return false;
		}
		if (_lastModule == null)
		{
			return true;
		}
		
		var cam = Camera.main;
		var camSize = cam.orthographicSize;
		var camWidth = camSize * 2 * cam.aspect;
		var camPos = cam.transform.position;

		return camPos.x + camWidth / 2f > _lastModule.transform.position.x;
	}

	private void LoadModule()
	{
		var count = moduleTemplates.Length;
		var index = UnityEngine.Random.Range(0, count);
		var template = moduleTemplates[index];
		var module = Instantiate(template);
		module.transform.parent = moduleRoot.transform;
		var lastPos = Vector3.zero;
		var lastWidth = 0f;
		if (_lastModule != null)
		{
			var box = _lastModule.GetComponent<BoxCollider2D>();
			lastPos = _lastModule.transform.position;
			lastWidth = box.size.x / 2;
		}

		var curWidth = module.GetComponent<BoxCollider2D>().size.x / 2;
		
		module.transform.position = new Vector3(lastPos.x + lastWidth + curWidth, 0, 0);
		_lastModule = module;
	}
	#endregion

}