using UnityEngine;
public class CharacterControl : MonoBehaviour
{
    Vector3 direction;
    public Transform player;
    float speed = 3;
    bool move = false;
    PlayfieldGeneration playfield;
    UIController uIController;
    public GameObject GameDifficultyButton;
    void Start()
    {
        uIController = GetComponent<UIController>();
        playfield = GetComponent<PlayfieldGeneration>();
        direction = Vector3.forward;
    }
    void Update()
    {
        if (Time.timeScale == 1)
        {
            if (move == false)
            {
                if (Input.GetMouseButtonUp(0))
                {
                    uIController.StartGame();
                    move = true;
                }
            }
            else
            {
                bool onTiles = playfield.CheckingDistanceToTiles();
                if (onTiles == false)
                {
                    NewGame();
                }

                bool collectedCrystal = playfield.CheckingDistanceToCrystals();
                if (collectedCrystal)
                {
                    uIController.AddOnePoint();
                }
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
                player.position += direction * speed * Time.deltaTime;
            }
        }
        else if (GameDifficultyButton.activeSelf == false)
        {
            Time.timeScale = 1;
        }
    }
    public void NewGame()
    {
        move = false;
        direction = Vector3.forward;
        uIController.GameOver();
        playfield.RestartGame();
    }
}