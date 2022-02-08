using UnityEngine;
public class Player
{
    private Transform player;
    private float speed = 3;
    private GameObject GO;
    public void Initialization()
    {
        GO = new GameObject();
        var find = GO.AddComponent<FindPlayer>();
        player = find.Find();
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
public class FindPlayer : MonoBehaviour
{
    public Transform Find()
    {
        return FindObjectOfType<TagPlayer>().transform;
    }
}
