using UnityEngine;
public class Main : MonoBehaviour
{
    enum Difficulty { Hard = 1, Normal = 2, Easy = 3 }
    int gameDifficulty = (int)Difficulty.Hard;
    Player player;
    GameState gameState;
    Playfield playfield;
    UIControler uIControler;
    Vector3 direction = Vector3.forward;
    Vector3 startPosition;
    public bool move = false;
    void Start()
    {
        player = new Player();
        player.Initialization();
        startPosition = player.GetPosition();

        gameState = new GameState();

        playfield = new Playfield();
        playfield.Initialization();

        uIControler = new UIControler();
        uIControler.Initialization();

        RestartGame();
    }
    public void ChangeDifficultyToEasy()
    {
        gameDifficulty = (int)Difficulty.Easy;
        uIControler.ShowFirstText();
        RestartGame();
        move = false;
    }
    public void ChangeDifficultyToNormal()
    {
        gameDifficulty = (int)Difficulty.Normal;
        uIControler.ShowFirstText();
        RestartGame();
        move = false;
    }
    public void ChangeDifficultyToHard()
    {
        gameDifficulty = (int)Difficulty.Hard;
        uIControler.ShowFirstText();
        RestartGame();
        move = false;
    }
    public void OpenMenu()
    {
        move = false;
        uIControler.ShowMenu();
    }
    void Update()
    {
        if (move == false)
        {
            if (uIControler.menu == false)
            {
                if (Input.GetMouseButtonUp(0))
                {
                    uIControler.HideUI();
                    move = true;
                }
            }
            else
            {
                if (Input.GetMouseButtonUp(0))
                {
                    uIControler.menu = false;
                }
            }
        }
        else
        {
            if (Input.GetMouseButtonUp(0))
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
            // if (collectedCrystal)
            // {
            //     uIController.AddOnePoint();
            // }
            player.Move(direction);
        }
        playfield.AddNextTile(player.GetPosition());
    }
    void RestartGame()
    {
        uIControler.ShowFirstText();
        move = false;
        direction = Vector3.forward;
        player.SetPosition(startPosition * gameDifficulty);
        playfield.StartGame(gameDifficulty);
    }
}