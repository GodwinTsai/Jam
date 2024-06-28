// ==================================================
// @Author: caiguorong
// @Maintainer: 
// @Date: 
// @Desc: 
// ==================================================
public class BadPlat : Plat
{
	protected override void OnTriggerAction()
	{
		GameFlowController.Instance.audioMgr.PlayEatBadZan();
		ThumbUpMgr.Instance.AddThumbDown(20);
	}
}