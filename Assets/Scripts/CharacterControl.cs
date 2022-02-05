using UnityEngine;
public class CharacterControl : MonoBehaviour
{
    Vector3 direction;
    public Transform player;
    float speed = 2;
    bool move = false;
    public PlayfieldGeneration playfield;
    void Start()
    {
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
            playfield.CheckingDistanceToCrystals();
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
        player.position = new Vector3(1, 0, 1.5f);
        playfield.RestartGame();
    }
}