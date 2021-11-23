using UnityEngine;

public class PlayerTrigger : MonoBehaviour
{
    public Room Room;
    public string PlayerTag = "Player";

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == PlayerTag)
        {
            // player in room
            if (Room)
            {
                Room.OnPlayerEntered();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == PlayerTag)
        {
            // player in room
            if (Room)
            {
                Room.OnPlayerExit();
            }
        }
    }
}