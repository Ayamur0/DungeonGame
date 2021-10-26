using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Assets.Game.Environment.Rooms.Maze;

public enum RoomType
{
    Default,
    Spawn,
    End
}

public class Room : MonoBehaviour
{
    public int Size = 20;
    public RoomType Type = RoomType.Default;
    public Direction Direction;
    public CellNeighborInfo NeighborInfos;
    public List<GameObject> Gates = new List<GameObject>();
    public Vector2 CellPosition = Vector2.zero;

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
        /*
        var player = other.GetComponent<Player>();
        if (player)
        {
            // player in room
        }
        */
    }
}