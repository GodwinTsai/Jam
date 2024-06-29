using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFlowStory : GameFlowUIBase
{
    public Story BeginStory;
    public Story StoryWinLevel1;
    public Story StoryLoseLevel1;

    public Story StoryWinLevel2;
    public Story StoryLoseLevel2;

    public Story StoryWinLevel3;
    public Story StoryLoseLevel3;

    private List<Story> _stories = new List<Story>();

    private void Awake()
    {
        _stories.Add(BeginStory);
        _stories.Add(StoryWinLevel1);
        _stories.Add(StoryLoseLevel1);
        _stories.Add(StoryWinLevel2);
        _stories.Add(StoryLoseLevel2);
        _stories.Add(StoryWinLevel3);
        _stories.Add(StoryLoseLevel3);
    }

    public void PlayStory(StoryType type)
    {
        foreach (Story story in _stories)
        {
            bool played = story.storyType == type;
            story.gameObject.SetActive(played);
            if (played)
            {
                story.Play();
            }
        }
    }
}