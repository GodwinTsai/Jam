using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioMgr : MonoBehaviour
{
    public AudioSource bgm;
    public AudioSource sound;
    public AudioClip Victory;
    public AudioClip Fail;
    public AudioClip ClickButton;
    public AudioClip EatZan;
    public AudioClip EatBadZan;
    public AudioClip Jump;
    
    public AudioClip Level1Bgm;
    public AudioClip Level2Bgm;
    public AudioClip Level3Bgm;
    public AudioClip Cutscene1Bgm;
    public AudioClip Cutscene2Bgm;
    public AudioClip Cutscene3Bgm;
    public AudioClip Fail2;
    public AudioClip Wining;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void PlayStoryBgm(StoryType type)
    {
        bgm.Stop();
        switch (type)
        {
            case StoryType.BeginStory:
                bgm.clip = Cutscene1Bgm;
                break;
            case StoryType.StoryWinLevel1:
                bgm.clip = Cutscene2Bgm;
                break;
            case StoryType.StoryLoseLevel1:
                bgm.clip = Fail2;
                break;
            case StoryType.StoryWinLevel2:
                bgm.clip = Cutscene3Bgm;
                break;
            case StoryType.StoryLoseLevel2:
                bgm.clip = Fail2;
                break;
            case StoryType.StoryWinLevel3:
                bgm.clip = Wining;
                break;
            case StoryType.StoryLoseLevel3:
                bgm.clip = Fail2;
                break;
        }

        bgm.Play();
    }

    public void PlayLevelBgm(int type)
    {
        bgm.Stop();
        switch (type)
        {
            case 1:
                bgm.clip = Level1Bgm;
                break;
            case 2:
                bgm.clip = Level2Bgm;
                break;
            case 3:
                bgm.clip = Level3Bgm;
                break;
        }
        bgm.Play();
    }

    public void PauseBgm()
    {
        bgm.Pause();
    }

    public void StopBgm()
    {
        bgm.Stop();
    }

    public void PlayJump()
    {
        sound.PlayOneShot(Jump);
    }
    public void PlayFail()
    {
        sound.PlayOneShot(Fail);
    }
    public void PlayClickButton()
    {
        sound.PlayOneShot(ClickButton);
    }
    public void PlayEatZan()
    {
        sound.PlayOneShot(EatZan);
    }
    
    public void PlayEatBadZan()
    {
        sound.PlayOneShot(EatBadZan);
    }
    
    public void PlayVictory()
    {
        sound.PlayOneShot(Victory);
    }
}