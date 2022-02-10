using UnityEngine;
public class Main : MonoBehaviour
{
    enum Difficulty { Hard = 1, Normal = 2, Easy = 3 }
    int gameDifficulty = (int)Difficulty.Hard;
    Player player;
    Playfield playfield;
    UIControler uIControler;
    Vector3 direction = Vector3.forward;
    Vector3 startPlayerPosition;
    public bool move = false;
    void Start()
    {
        player = new Player();
        player.Initialization();
        startPlayerPosition = player.GetPosition();

        playfield = new Playfield();
        playfield.Initialization();

        uIControler = new GameObject().AddComponent<UIControler>();
        uIControler.Initialization();

        RestartGame();
    }
    void Update()
    {
        if (move == true)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (direction == Vector3.right)
                {
                    direction = Vector3.forward;
                }
                else
                {
                    direction = Vector3.right;
                }
            }
            bool onTiles = playfield.CheckingDistanceToTiles(player.GetPosition());
            if (onTiles == false)
            {
                RestartGame();
            }
            bool collectedCrystal = playfield.CheckingDistanceToCrystals(player.GetPosition());
            if (collectedCrystal)
            {
                uIControler.AddOnePoint();
            }
            player.Move(direction);
        }
        playfield.AddNextTile(player.GetPosition());
    }
    public void ChangeDifficultyToEasy()
    {
        gameDifficulty = (int)Difficulty.Easy;
        RestartGame();
    }
    public void ChangeDifficultyToNormal()
    {
        gameDifficulty = (int)Difficulty.Normal;
        RestartGame();
    }
    public void ChangeDifficultyToHard()
    {
        gameDifficulty = (int)Difficulty.Hard;
        RestartGame();
    }
    public void OpenMenu()
    {
        move = false;
        uIControler.ShowMenu();
    }
    public void StartGame()
    {
        uIControler.HideFirstText();
        move = true;
    }
    void RestartGame()
    {
        direction = Vector3.forward;
        player.SetPosition(startPlayerPosition * gameDifficulty);
        playfield.StartGame(gameDifficulty);
        uIControler.ShowFirstText();
        uIControler.HideMenu();
        uIControler.Restart();
        move = false;
    }
}