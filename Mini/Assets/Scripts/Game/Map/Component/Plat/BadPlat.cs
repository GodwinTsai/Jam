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
		GameFlowController.Ins.audioMgr.PlayEatBadZan();
		ThumbUpMgr.Ins.AddThumbDown(20);
	}
}