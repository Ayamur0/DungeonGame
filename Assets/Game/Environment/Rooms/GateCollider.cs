using UnityEngine;

public class GateCollider : MonoBehaviour
{
    public GameObject SpawnPostion;

    private void OnTriggerEnter(Collider other)
    {
        var room = GetComponentInParent<Room>();
        if (other.gameObject.tag == "Player" && !room.GatesClosed)
        {
            // for testing only
            var simpleController = other.gameObject.GetComponent<SimplePlayerController>();
            if (simpleController)
            {
                simpleController.enabled = false;
            }

            var playerController = other.gameObject.GetComponent<PlayerController>();
            if (playerController)
            {
                playerController.enabled = false;
            }

            var playerObj = other.gameObject;
            var pos = playerObj.transform.position;
            var spawnPos = SpawnPostion.transform.position;
            playerObj.transform.position = new Vector3(spawnPos.x, pos.y, spawnPos.z);

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            var simpleController = other.gameObject.GetComponent<SimplePlayerController>();
            if (simpleController)
            {
                simpleController.enabled = true;
            }

            var playerController = other.gameObject.GetComponent<PlayerController>();
            if (playerController)
            {
                playerController.enabled = true;
            }
        }
    }
}
