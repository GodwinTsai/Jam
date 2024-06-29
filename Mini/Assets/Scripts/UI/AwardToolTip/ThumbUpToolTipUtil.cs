// ==================================================
// @Author: caiguorong
// @Maintainer: 
// @Date: 
// @Desc: 
// ==================================================
using UnityEngine;

public class ThumbUpToolTipUtil
{
	#region Private Fields
	private const string PREFAB_PATH = "UI/ToolTip/ThumbUpToolTip";
	private static Camera _mainCamera;
	
	private static Vector2 minLocalPos = new Vector2(-900, 200);
	private static Vector2 maxLocalPos = new Vector2(-500, 400);
	#endregion

	#region Public Methods
	public static void ShowThumbUpToolTipLocalPos(EnumScoreType scoreType, EnumScoreNumType numType)
	{
		var localPos = GetRandomLocalPos();
		// MTDebug.Log(Color.yellow, $"[ThumbUpToolTip]111ShowThumbUpToolTip--LocalPos--");

		var component = NewThumbUpToolTipComponent();
		if (component == null)
		{
			return;
		}
		component.transform.localPosition = localPos;
		component.Show(scoreType, numType);
	}

	private static Vector2 GetRandomLocalPos()
	{
		var x = UnityEngine.Random.Range(minLocalPos.x, maxLocalPos.x);
		var y = UnityEngine.Random.Range(minLocalPos.y, maxLocalPos.y);
		return new Vector2(x, y);
	}

	#endregion

	#region Private Methods
	private static Vector2 GetLocalPos(Transform transform, Vector3 worldPos)
	{
		var screenPos = MainCamera.WorldToScreenPoint(worldPos);
		screenPos.z = 0;
		if (RectTransformUtility.ScreenPointToLocalPointInRectangle(transform.parent as RectTransform, screenPos, MainCamera, out Vector2 localPos))
		{
			return localPos;
		}
		return Vector2.zero;
	}
	
	private static Camera MainCamera
	{
		get
		{
			if (_mainCamera == null)
			{
				_mainCamera = Camera.main;
			}
			return _mainCamera;
		}
	}

	private static ThumbUpToolTipComponent NewThumbUpToolTipComponent()
	{
		var prefab = ResouceUtil.LoadPrefab(PREFAB_PATH);
		if (prefab == null)
		{
			return null;
		}

		var parent = GameFlowController.Instance.GameFlow.tipsLayer.transform;
		prefab.transform.SetParent(parent, false);
		
		var component = prefab.GetComponent<ThumbUpToolTipComponent>();
		return component;
	}
	#endregion
}