using Assets.Game.Environment.Rooms;
using System.Collections.Generic;
using UnityEngine;

public enum RoomType
{
    Spawn,
    Exit,
    Shop,
    Battle,
    Explore,
}

public class Room : MonoBehaviour
{
    public int Size = 20;
    public RoomType Type = RoomType.Battle;
    public CellNeighborInfo NeighborInfos;
    public List<GameObject> Gates = new List<GameObject>();
    public Vector2 CellPosition = Vector2.zero;
    public float GateSize = 2.6f;

    // Start is called before the first frame update
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            OpenGates();
            Debug.Log("OPEN GATES");
        }
    }

    public void OpenGates()
    {
        foreach (var gate in Gates)
        {
            gate.transform.position -= new Vector3(0, GateSize, 0);
        }
    }

    public void CloseGates()
    {
        foreach (var gate in Gates)
        {
            gate.transform.position += new Vector3(0, GateSize, 0);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        /*
        var player = other.GetComponent<Player>();
        if (player)
        {
            // player in room
        }
        */
    }
}