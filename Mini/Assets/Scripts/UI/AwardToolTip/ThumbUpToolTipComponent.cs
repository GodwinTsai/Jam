// ==================================================
// @Author: caiguorong
// @Maintainer: caiguorong
// @Date: 
// @Desc: 
// ==================================================
using UnityEngine;
using UnityEngine.UI;

public class ThumbUpToolTipComponent : MonoBehaviour
{
	#region Mono Fields

	public GameObject thumbUp;
	public GameObject love;
	public GameObject thumbDown;
	public GameObject loveBroken;
	public GameObject add;
	public GameObject minus;
	
	public Text numText;

	public float flyTime = 1.1f;
	#endregion

	#region Private Methods

	public void Show(EnumScoreType scoreType, EnumScoreNumType numType)
	{
		// MTDebug.Log(Color.yellow, $"[AwardToolTip]222ShowAwardToolTip--Show:score{score}");
		// numText.text = score >= 0 ? $"+{score}" : $"{score}";
		// this.Wait(flyTime).Then(FlyEnd);
		thumbUp.SetActive(scoreType == EnumScoreType.ThumbUp);
		love.SetActive(scoreType == EnumScoreType.Love);
		thumbDown.SetActive(scoreType == EnumScoreType.ThumbDown);
		loveBroken.SetActive(scoreType == EnumScoreType.LoveBroken);
		add.SetActive(numType == EnumScoreNumType.Add);
		minus.SetActive(numType == EnumScoreNumType.Minus);
		
		Invoke("FlyEnd", flyTime);
	}

	private void FlyEnd()
	{
		// MTDebug.Log(Color.yellow, $"[AwardToolTip]333ShowAwardToolTip--End");
		Destroy(gameObject);
	}
	#endregion

}