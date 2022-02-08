using UnityEngine;
public class Main : MonoBehaviour
{
    Player player;
    Playfield2 playfield;
    Vector3 direction = Vector3.forward;
    public GameState gameState;
    Vector3 startPosition;
    // UI ui;
    void Start()
    {
        gameState = GetComponent<GameState>();
        playfield = GetComponent<Playfield>().ttt();

        player = new Player();
        player.player = FindObjectOfType<TagPlayer>().transform;
        startPosition = player.player.position;
        // ui = new UI();
        playfield.gameState = gameState;
        playfield.prefabTile = Resources.Load<Transform>("Tile");
        playfield.prefabcrystal = Resources.Load<Transform>("Crystal");
        playfield.parentTiles = new GameObject().transform;
        playfield.parentCrystals = new GameObject().transform;
        playfield.sizeTilse.Add(new Vector3(1, 1, 1));
        playfield.sizeTilse.Add(new Vector3(2, 2, 2));
        playfield.sizeTilse.Add(new Vector3(3, 3, 3));


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
            bool onTiles = playfield.CheckingDistanceToTiles(player.player.position);
            if (onTiles == false)
            {
                RestartGame();
            }
            bool collectedCrystal = playfield.CheckingDistanceToCrystals(player.player.position);
            // if (collectedCrystal)
            // {
            //     uIController.AddOnePoint();
            // }
            player.Move(direction);
        }
        playfield.DistanceСheck(player.player.position);
    }
    void RestartGame()
    {
        direction = Vector3.forward;
        player.player.position = startPosition * gameState.gameDifficulty;
        playfield.StartGame();
    }
}