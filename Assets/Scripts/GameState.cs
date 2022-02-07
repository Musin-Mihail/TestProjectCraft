using UnityEngine;
namespace Game
{
    public class GameState : MonoBehaviour
    {
        enum Difficulty { Hard = 1, Normal = 2, Easy = 3 }
        [HideInInspector] public int gameDifficulty = (int)Difficulty.Easy;
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
    }
}