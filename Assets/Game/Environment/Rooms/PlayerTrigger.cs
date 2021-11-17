using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

public class PlayerTrigger : MonoBehaviour
{
    public Room Room;
    public string PlayerTag = "Player";

    // Start is called before the first frame update
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
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