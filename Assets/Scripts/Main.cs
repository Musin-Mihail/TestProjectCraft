using UnityEngine;
namespace Game
{
    public class Main : MonoBehaviour
    {
        Player player;
        Playfield playfield;
        Vector3 direction = Vector3.forward;
        GameState gameState;
        Vector3 startPosition;
        bool move = false;
        // UI ui;
        void Start()
        {
            gameState = GetComponent<GameState>();
            playfield = GetComponent<Playfield>();
            player = new Player();
            player.player = FindObjectOfType<TagPlayer>().transform;
            startPosition = player.player.position;
            // ui = new UI();
            playfield.AddResources();
            RestartGame();
        }
        void Update()
        {
            if (move == false)
            {
                if (Input.GetMouseButtonUp(0))
                {
                    // uIController.StartGame();
                    move = true;
                }
            }
            else
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
                bool onTiles = playfield.CheckingDistanceToTiles(player.player.position);
                if (onTiles == false)
                {
                    RestartGame();
                }
                // bool collectedCrystal = playfield.CheckingDistanceToCrystals();
                // if (collectedCrystal)
                // {
                //     uIController.AddOnePoint();
                // }
                player.Move(direction);
            }
        }
        void RestartGame()
        {
            direction = Vector3.forward;
            player.player.position = startPosition * gameState.gameDifficulty;
            playfield.StartGame();
        }
    }
}