
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Minimap : MonoBehaviour
{
    public LevelManager LevelManager;
    public LevelGenerator LevelGenerator;
    public GameObject RoomSprite;
    public GameObject Container;

    public Image[,] RoomImages;

    private List<GameObject> activeMinimapTiles = new List<GameObject>();

    private Vector2 lastPosition = Vector2.negativeInfinity;

    // Start is called before the first frame update
    void Start()
    {
        if (LevelManager != null && LevelGenerator != null)
        {
            this.LevelManager.ActiveRoomChanged += ActiveRoomChanged;
            this.LevelManager.NextStageLoaded += NextStageLoaded;

            InitMinimap();
        }
    }

    private void InitMinimap()
    {
        var rooms = this.LevelGenerator.GetRooms();
        this.RoomImages = new Image[this.LevelGenerator.Settings.MapWidth, this.LevelGenerator.Settings.MapHeight];
        for (int x = 0; x < RoomImages.GetLength(0) - 1; x++)
        {
            for (int y = 0; y < RoomImages.GetLength(1) - 1; y++)
            {
                if (this.RoomImages[x, y] == null)
                {
                    var spriteObj = Instantiate(RoomSprite, Container.transform);
                    var imageCmp = spriteObj.GetComponent<Image>();
                    this.RoomImages[x, y] = imageCmp;
                }
                // set to transparent
                this.RoomImages[x, y].color = new Color(0, 0, 0, 0);
            }
        }
    }

    private void ActiveRoomChanged(GameObject prevRoom, GameObject newRoom)
    {
        var roomCmp = newRoom.GetComponent<Room>();
        var cellPosition = newRoom.GetComponent<Room>().CellPosition;
        var roomImageCmp = RoomImages[(int)cellPosition.x, (int)cellPosition.y];

        if (lastPosition.x >= 0 && lastPosition.y >= 0)
        {
            if (lastPosition != cellPosition)
            {
                var lastRoomCmp = RoomImages[(int)lastPosition.x, (int)lastPosition.y];
                if (prevRoom)
                {
                    var lastRoom = prevRoom.GetComponent<Room>();
                    if (lastRoom != null)
                    {
                        if (lastRoom.Type == RoomType.Battle || lastRoom.Type == RoomType.Explore)
                        {
                            lastRoomCmp.color = Color.gray;
                        }
                    }
                }
            }

        }

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

        activeMinimapTiles.Add(roomImageCmp.gameObject);

        lastPosition = cellPosition;
    }

    private void NextStageLoaded()
    {
        foreach (var minimapTile in activeMinimapTiles)
        {
            var imageCmp = minimapTile.GetComponent<Image>();
            imageCmp.color = new Color(0, 0, 0, 0);
        }

        activeMinimapTiles.Clear();
    }
}
