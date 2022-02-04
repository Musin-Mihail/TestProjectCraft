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
            bool onTiles = false;
            foreach (var tile in playfield.poolTiles)
            {
                if (Vector3.Distance(tile.position, player.position) < 0.7f)
                {
                    onTiles = true;
                    break;
                }
            }
            if (onTiles == false)
            {
                Losing();
            }
            foreach (var crystal in playfield.Сrystals)
            {
                if (crystal.gameObject.activeSelf)
                {
                    if (Vector3.Distance(crystal.position, player.position) < 1.1f)
                    {
                        crystal.gameObject.SetActive(false);
                    }
                }
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
        player.GetChild(1).GetComponent<SpriteRenderer>().material.color = Color.red;
        Time.timeScale = 0;
    }
}