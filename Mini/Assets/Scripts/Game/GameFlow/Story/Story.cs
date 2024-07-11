using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class Story : MonoBehaviour
{
    // 播完剧情以后，开始的关卡类型
    public Level EnterLevel;
    public StoryType storyType;
    private List<SpriteInfo> imageList = new List<SpriteInfo>();
    private int _curPageIndex = 0;
    public bool isAutoPlay = true;
    private Coroutine coroutine;
    public GameObject storyPagePrefab;
    private Image _display;

    struct SpriteInfo
    {
        public Sprite sprite;
        public int index;
        public float delayTime;
    }

    private void Awake()
    {
        // 创建一个新的 Image 对象
        GameObject imageObject = Instantiate(storyPagePrefab, transform);
        _display = imageObject.GetComponent<Image>();
        imageObject.GetComponent<StoryPage>().Story = this;
        imageList.Clear();

        Sprite[] storyImages = Resources.LoadAll<Sprite>($"Story/{storyType.ToString()}");
        foreach (Sprite image in storyImages)
        {
            string[] arr = image.name.Split('-');
            SpriteInfo info = new SpriteInfo();
            info.sprite = image;
            info.index = int.Parse(arr[0]);
            info.delayTime = int.Parse(arr[1]);
            imageList.Add(info);
        }

        imageList.Sort((a, b) => { return a.index - b.index; });
    }

    public void Play()
    {
        this._curPageIndex = 0;
        this.RefreshPage();
        if (isAutoPlay)
        {
            coroutine = StartCoroutine(AutoPlayStory());
        }
    }

    public void HandlePageClick()
    {
        if (isAutoPlay)
        {
            return;
        }

        NextPage();
    }

    private void NextPage()
    {
        this._curPageIndex++;
        if (this._curPageIndex < imageList.Count)
        {
            this.RefreshPage();
        }
        else
        {
            OnEnd();
        }
    }

    private void OnEnd()
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
            coroutine = null;
        }

        if (storyType == StoryType.StoryWinLevel3)
        {
            GameFlowController.Ins.EnterFlow(GameFlowEnum.GameFlowMenu);
        }
        else
        {
            switch (storyType)
            {
                case StoryType.BeginStory:
                    GameFlowController.Ins.ShowHTP();
                    break;
                case StoryType.StoryWinLevel1:
                case StoryType.StoryWinLevel2:
                    GameFlowController.Ins.EnterNextLevel();
                    break;
                case StoryType.StoryWinLevel3:
                    GameFlowController.Ins.EnterFlow(GameFlowEnum.GameFlowMenu);
                    GameFlowController.Ins.audioMgr.StopBgm();
                    break;
                case StoryType.StoryLoseLevel1:
                case StoryType.StoryLoseLevel2:
                case StoryType.StoryLoseLevel3:
                    GameFlowController.Ins.RetryCurLevel();
                    break;
            }
        }
    }

    private void RefreshPage()
    {
        if (_curPageIndex < imageList.Count)
        {
            _display.sprite = imageList[_curPageIndex].sprite;
        }
    }

    IEnumerator AutoPlayStory()
    {
        while (this._curPageIndex < imageList.Count)
        {
            float delaytime = imageList[_curPageIndex].delayTime * 0.001f;
            // 等待 1 秒
            yield return new WaitForSeconds(delaytime);
            NextPage();
        }
    }
}