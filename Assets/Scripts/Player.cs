using UnityEngine;
public class Player
{
    public Transform player;
    float speed = 3;
    public void Move(Vector3 direction)
    {
        player.position += direction * speed * Time.deltaTime;
    }
}