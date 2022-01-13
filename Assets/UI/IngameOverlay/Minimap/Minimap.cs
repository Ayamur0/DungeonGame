
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class Minimap : MonoBehaviour
{
    public LevelManager LevelManager;
    public LevelGenerator LevelGenerator;
    public GameObject RoomSprite;
    public GameObject CircleObj;

    public Image[,] RoomImages;

    private List<GameObject> activeMinimapTiles = new List<GameObject>();

    private Vector2 lastPosition = Vector2.negativeInfinity;

    // Start is called before the first frame update
    void Start()
    {
        if (LevelManager != null && LevelGenerator != null)
        {
            this.LevelGenerator.RoomsGenerated = RoomsGenerated;
            this.LevelManager.ActiveRoomChanged += ActiveRoomChanged;
            this.LevelManager.NextStageLoaded += NextStageLoaded;
        }
    }

    private void NextStageLoaded()
    {
        for (int x = 0; x < RoomImages.GetLength(0) - 1; x++)
        {
            for (int y = 0; y < RoomImages.GetLength(1) - 1; y++)
            {
                RoomImages[x, y] = null;
            }
        }

        for (int i = 0; i < activeMinimapTiles.Count -1; i++)
        {
            Destroy(activeMinimapTiles[i]);
        }

        activeMinimapTiles.Clear();

        RoomsGenerated();
        ActiveRoomChanged();
    }

    private void ActiveRoomChanged()
    {
        var room = this.LevelManager.GetActiveRoom();
        var cellPosition = room.GetComponent<Room>().CellPosition;

        if (this.RoomImages != null)
        {
            var roomCmp = room.GetComponent<Room>();

            if (lastPosition != cellPosition)
            {
                var lastRoomCmp = RoomImages[(int)lastPosition.x, (int)lastPosition.y];
                var lastRoomObj = this.LevelGenerator.GetRooms()[(int)lastPosition.x, (int)lastPosition.y];
                if (lastRoomObj)
                {
                    var lastRoom = lastRoomObj.GetComponent<Room>();
                    if (lastRoom)
                    {
                        if (lastRoom.Type == RoomType.Battle || lastRoom.Type == RoomType.Explore)
                        {
                            lastRoomCmp.color = Color.gray;
                        }
                    }
                }

                var moveDirection = lastPosition - cellPosition;
                for (int x = 0; x < RoomImages.GetLength(0) - 1; x++)
                {
                    for (int y = 0; y < RoomImages.GetLength(1) - 1; y++)
                    {
                        var roomImage = RoomImages[x, y];
                        if (roomImage != null)
                        {
                            roomImage.transform.position += new Vector3(moveDirection.x, moveDirection.y) * 16f;
                        }
                    }
                }
            }

            var roomImageCmp = RoomImages[(int)cellPosition.x, (int)cellPosition.y];
            roomImageCmp.enabled = true;

            if (roomCmp.Type == RoomType.Spawn)
            {
                roomImageCmp.color = Color.green;
            }
            else if (roomCmp.Type == RoomType.Shop)
            {
                roomImageCmp.color = Color.yellow;
            }
            else if (roomCmp.Type == RoomType.Exit)
            {
                roomImageCmp.color = Color.blue;
            }
            else
            {
                roomImageCmp.color = Color.white;
            }
        }

        lastPosition = cellPosition;
    }   

    private void RoomsGenerated()
    {
        var rooms = this.LevelGenerator.GetRooms();

        if (rooms != null)
        {
            this.RoomImages = new Image[this.LevelGenerator.Settings.MapWidth, this.LevelGenerator.Settings.MapHeight];
            for (int x = 0; x < rooms.GetLength(0) - 1; x++)
            {
                for (int y = 0; y < rooms.GetLength(1) - 1; y++)
                {
                    var room = rooms[x, y];
                    if (room != null)
                    {
                        var roomCmp = room.GetComponent<Room>();
                        if (RoomSprite != null && roomCmp != null)
                        {
                            var spriteObj = Instantiate(RoomSprite, transform);
                            spriteObj.transform.position = (CircleObj.transform.position + new Vector3(x, y, 0f) * 24f) - new Vector3(rooms.GetLength(0) * 0.5f, rooms.GetLength(1) * 0.5f, 0f) * 24f;
                            var imageCmp = spriteObj.GetComponent<Image>();
                            imageCmp.enabled = false;
                            imageCmp.color = Color.gray;

                            activeMinimapTiles.Add(imageCmp.gameObject);

                            this.RoomImages[x, y] = imageCmp;
                        }
                    }
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
