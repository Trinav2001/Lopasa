using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform player;
    void Update()
    {
        transform.position = new Vector3(player.position.x+4, player.position.y+2, transform.position.z);
    }// Player is shown in the Left Bottom in the Camera
}
