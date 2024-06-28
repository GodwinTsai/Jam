using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameFlow : MonoBehaviour
{
    public GameFlowStory Stroy;
    public GameFlowMenu Menu;
    public GameFlowPlayUI PlayUI;
    private List<GameFlowUIBase> AllFlows = new List<GameFlowUIBase>();
    public GameObject tipsLayer;

    public GameFlowEnum CurType;

    // Start is called before the first frame update
    void Start()
    {
        this.AllFlows.Add(this.Stroy);
        this.AllFlows.Add(this.Menu);
        this.AllFlows.Add(this.PlayUI);
        // LoadLevel(DataCenter.Instance.GetLevelData(Level.Level1));
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void EnterFlow(GameFlowEnum flowType)
    {
        CurType = flowType;
        foreach (GameFlowUIBase gameFlowUIBase in AllFlows)
        {
            gameFlowUIBase.gameObject.SetActive(gameFlowUIBase.Type == flowType);
        }

        switch (flowType)
        {
            case GameFlowEnum.Placeholder:
                break;
            case GameFlowEnum.GameFlowStory:
                break;
            case GameFlowEnum.GameFlowMenu:
                break;
            case GameFlowEnum.GameFlowPlayUI:
                PlayUI.BeginLevel();
                break;
        }
    }
    public void PlayStory(StoryType storyType)
    {
        EnterFlow(GameFlowEnum.GameFlowStory);
        Stroy.PlayStory(storyType);
    }

    public void ShowHTP()
    {
        EnterFlow(GameFlowEnum.GameFlowMenu);
        Menu.ShowHTP();
    }
}