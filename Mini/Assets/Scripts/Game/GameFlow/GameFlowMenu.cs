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
        GameFlowController.Instance.PlayStory(StoryType.BeginStory);
        // GameFlowController.Instance.PlayStory(StoryType.StoryWinLevel1);
        GameFlowController.Instance.audioMgr.PlayClickButton();
    }

    public void OnExitBtnClick()
    {
        GameFlowController.Instance.audioMgr.PlayClickButton();
        GameFlowController.Instance.QuitGame();
    }

    /// <summary>
    /// htp界面，点击我会了 ，开始游戏
    /// </summary>
    public void ClickHtpBtn()
    {
        HTP.SetActive(false);
        LevelMgr.Instance.ResetLevel();
        GameFlowController.Instance.audioMgr.PlayClickButton();
        GameFlowController.Instance.EnterNextLevel();
    }
    
    public void ShowHTP()
    {
        HTP.SetActive(true);
    }

}