using UnityEngine;
public class CharacterControl : MonoBehaviour
{
    Vector3 direction;
    public Transform player;
    float speed = 3;
    bool move = false;
    PlayfieldGeneration playfield;
    UIController uIController;
    void Start()
    {
        uIController = GetComponent<UIController>();
        playfield = GetComponent<PlayfieldGeneration>();
        direction = Vector3.forward;
    }
    void Update()
    {
        if (move == false)
        {
            if (Input.GetMouseButtonDown(0))
            {
                move = true;
            }
        }
        else
        {
            bool onTiles = playfield.CheckingDistanceToTiles();
            if (onTiles == false)
            {
                Losing();
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
    void Losing()
    {
        move = false;
        direction = Vector3.forward;
        uIController.GameOver();
        playfield.RestartGame();
    }
}