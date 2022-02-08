using UnityEngine;
public class Player: MonoBehaviour
{
    private Transform player;
    float speed = 3;
    public void Initialization()
    {
        player = FindObjectOfType<TagPlayer>().transform;
    }
    public void Move(Vector3 direction)
    {
        player.position += direction * speed * Time.deltaTime;
    }
    public Vector3 SetPosition()
    {
        return player.position;
    }
    public void GetPosition(Vector3 newPosition)
    {
        player.position = newPosition;
    }
}