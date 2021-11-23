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
    public bool PlayerInRoom { get; private set; }
    public string PlayerTag = "Player";
    public bool Visited = false;
    public LevelManager LevelManager;

    // Start is called before the first frame update
    private void Start()
    {
    }

    public void OpenGates()
    {
        foreach (var gate in Gates)
        {
            var gatePos = gate.transform.position;
            gate.transform.position = new Vector3(gatePos.x, GateSize, gatePos.z);
        }
    }

    public void CloseGates()
    {
        foreach (var gate in Gates)
        {
            var gatePos = gate.transform.position;
            gate.transform.position = new Vector3(gatePos.x, -GateSize, gatePos.z);
        }
    }

    public void OnPlayerEntered()
    {
        Debug.Log("ENTER");
        this.PlayerInRoom = true;

        this.LevelManager.SetActiveRoom(this.gameObject);

        if (!Visited)
            CloseGates();
    }

    public void OnPlayerExit()
    {
        Debug.Log("Exit");
        this.PlayerInRoom = false;
        OpenGates();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == PlayerTag)
        {
            OnPlayerEntered();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == PlayerTag)
        {
            OnPlayerExit();
        }
    }
}