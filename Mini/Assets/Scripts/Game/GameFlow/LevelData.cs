public enum Level
{
    Level1,
    Level2,
    Level3,
}

public class LevelData
{
    public Level level;
    public string path;
    public StoryType winStory;
    public StoryType loseStory;

    public LevelData(
        Level level,
        string path,
        StoryType winStory,
        StoryType loseStory
    )
    {
        this.level = level;
        this.path = path;
        this.winStory = winStory;
        this.loseStory = loseStory;
    }
}