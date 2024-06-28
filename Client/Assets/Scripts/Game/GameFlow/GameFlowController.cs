using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFlowController : SingletonMonobehaviourAuto<GameFlowController>
{
    public AudioMgr audioMgr;
    public GameFlow GameFlow;

    void Awake()
    {
        EventManager.RegisterEvent(EventConst.EventLevelSuccess, handleWin);
        audioMgr = GameObject.Find("AudioMgr").GetComponent<AudioMgr>();
    }

    // Start is called before the first frame update
    void Start()
    {
        EnterFlow(GameFlowEnum.GameFlowMenu);
    }

    public void Init(GameFlow gf)
    {
        GameFlow = gf;
    }

    public void handleWin()
    {
        audioMgr.PlayVictory();
        WinCurrentLevel();
    }

    public void EnterFlow(GameFlowEnum flowType)
    {
        GameFlow.EnterFlow(flowType);
    }

    public void EnterNextLevel()
    {
        DataCenter.Instance.ResetData();
        EnterFlow(GameFlowEnum.GameFlowPlayUI);
        LevelMgr.Instance.EnterNextLevel();
        audioMgr.PlayLevelBgm(LevelMgr.Instance.curLevel);
    }

    public void RetryCurLevel()
    {
        DataCenter.Instance.ResetData();
        EnterFlow(GameFlowEnum.GameFlowPlayUI);
        LevelMgr.Instance.RetryCurLevel();
        audioMgr.PlayLevelBgm(LevelMgr.Instance.curLevel);
    }

    public void PlayStory(StoryType type)
    {
        GameFlow.PlayStory(type);
        audioMgr.PlayStoryBgm(type);
    }

    public void WinCurrentLevel()
    {
        switch (LevelMgr.Instance.curLevel)
        {
            case 1:
                PlayStory(StoryType.StoryWinLevel1);
                break;
            case 2:
                PlayStory(StoryType.StoryWinLevel2);
                break;
            case 3:
                PlayStory(StoryType.StoryWinLevel3);
                break;
        }
    }

    public void LoseCurrentLevel()
    {
        switch (LevelMgr.Instance.curLevel)
        {
            case 1:
                PlayStory(StoryType.StoryLoseLevel1);
                break;
            case 2:
                PlayStory(StoryType.StoryLoseLevel2);
                break;
            case 3:
                PlayStory(StoryType.StoryLoseLevel3);
                break;
        }
    }

    public void ShowHTP()
    {
        audioMgr.bgm.Stop();
        GameFlow.ShowHTP();
    }

    public void QuitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    public override bool DestroyOnReboot()
    {
        return false;
    }
}