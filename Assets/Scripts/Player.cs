using UnityEngine;
public class Player
{
    private Transform player;
    private float speed = 3;
    public void Initialization()
    {
        var find = new GameObject().AddComponent<FindPlayer>();
        player = find.Find();
    }
    public void Move(Vector3 direction)
    {
        player.position += direction * speed * Time.deltaTime;
    }
    public Vector3 GetPosition()
    {
        return player.position;
    }
    public void SetPosition(Vector3 newPosition)
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
