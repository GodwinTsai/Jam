using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameFlowMenu : GameFlowUIBase
{
    public Button StartBtn;

    public Button EndBtn;
    public GameObject HTP;

    /// <summary>
    /// 主界面，开始按钮，点击，播放剧情
    /// </summary>
    public void OnStartBtnClick()
    {
        GameFlowController.Ins.PlayStory(StoryType.BeginStory);
        // GameFlowController.Ins.PlayStory(StoryType.StoryWinLevel1);
        GameFlowController.Ins.audioMgr.PlayClickButton();
    }

    public void OnExitBtnClick()
    {
        GameFlowController.Ins.audioMgr.PlayClickButton();
        GameFlowController.Ins.QuitGame();
    }

    /// <summary>
    /// htp界面，点击我会了 ，开始游戏
    /// </summary>
    public void ClickHtpBtn()
    {
        HTP.SetActive(false);
        LevelMgr.Ins.ResetLevel();
        GameFlowController.Ins.audioMgr.PlayClickButton();
        GameFlowController.Ins.EnterNextLevel();
    }
    
    public void ShowHTP()
    {
        HTP.SetActive(true);
    }

}