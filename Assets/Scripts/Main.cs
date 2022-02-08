using UnityEngine;
public class Main : MonoBehaviour
{
    Player player;
    GameState gameState;
    Playfield playfield;
    Vector3 direction = Vector3.forward;
    Vector3 startPosition;
    void Start()
    {
        player = new Player();
        player.Initialization();
        startPosition = player.SetPosition();

        gameState = new GameState();

        playfield = new Playfield();
        playfield.Initialization();

        RestartGame();
    }
    void Update()
    {
        if (gameState.move == false)
        {
            if (gameState.menu == false)
            {
                if (Input.GetMouseButtonUp(0))
                {
                    // uIController.StartGame();
                    gameState.move = true;
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
            bool onTiles = playfield.CheckingDistanceToTiles(player.SetPosition());
            if (onTiles == false)
            {
                RestartGame();
            }
            bool collectedCrystal = playfield.CheckingDistanceToCrystals(player.SetPosition());
            // if (collectedCrystal)
            // {
            //     uIController.AddOnePoint();
            // }
            player.Move(direction);
        }
        playfield.DistanceСheck(player.SetPosition());
    }
    void RestartGame()
    {
        gameState.move = false;
        direction = Vector3.forward;
        player.GetPosition(startPosition * gameState.gameDifficulty);

        playfield.StartGame(gameState.gameDifficulty);
    }
}