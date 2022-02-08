public class GameState
{
    enum Difficulty { Hard = 1, Normal = 2, Easy = 3 }
    public int gameDifficulty = (int)Difficulty.Easy;
    public bool move = false;
    public bool menu = false;
    public void ChangeDifficultyToEasy()
    {
        gameDifficulty = (int)Difficulty.Easy;
    }
    public void ChangeDifficultyToNormal()
    {
        gameDifficulty = (int)Difficulty.Normal;
    }
    public void ChangeDifficultyToHard()
    {
        gameDifficulty = (int)Difficulty.Hard;
    }
    public void OpenMenu()
    {
        menu = true;
        move = false;
    }
}