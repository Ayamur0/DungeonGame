using UnityEngine;

public class PlayerTrigger : MonoBehaviour
{
    public Room Room;
    public string PlayerTag = "Player";

    // Start is called before the first frame update
    private void Start()
    {
    }

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