using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using UnityEngine;

public class LevelGeneration : MonoBehaviour
{
    [SerializeField]
    private Transform[] startingPostions;

    [SerializeField]
    public GameObject[] Rooms;

    [SerializeField]
    private LayerMask roomLayerMask;

    private List<GameObject> createdRooms;

    public bool StopGeneration { get; private set; } = false;
    private bool generationComplete = false;

    private int downCounter = 0;
    private int direction;

    public float moveAmount;
    public float minX;
    public float maxX;
    public float minY;

    private void Start()
    {
        var randStartingPos = Random.Range(0, startingPostions.Length);
        transform.position = startingPostions[randStartingPos].position;

        createdRooms = new List<GameObject>();
        createdRooms.Add(Instantiate(Rooms[0], transform.position, Quaternion.identity));
        direction = Random.Range(1, 6);
    }

    private void Update()
    {
        if (generationComplete)
            Destroy(gameObject);

        if(!StopGeneration)
            Move();
        else
        {
            foreach(var room in createdRooms)
            {
                if(room != null)
                    Destroy(room.gameObject.GetComponent<BoxCollider2D>());
            }

            generationComplete = true;
        }
    }
   
    private void Move()
    {
        if(direction == 1 || direction == 2)
        {
            CreateRoomToTheRight();
        }
        else if (direction == 3 || direction == 4)
        {
            CreateRoomToTheLeft();
        }
        else if (direction == 5)
        {
            CreateRoomDownwards();
        }
    }

    private void CreateRoomToTheLeft()
    {
        downCounter = 0;

        if (transform.position.x > minX)
        {
            UpdatePositionAndCreateRoom(-moveAmount, 3, 6, false, RoomType.Any);
        }
        else
        {
            direction = 5;
        }
    }

    private void CreateRoomToTheRight()
    {
        downCounter = 0;

        if (transform.position.x < maxX)
        {
            UpdatePositionAndCreateRoom(moveAmount, 1, 6, false, RoomType.Any);

            if (direction == 3)
                direction = 2;
            else if (direction == 4)
                direction = 5;
        }
        else
        {
            direction = 5;
        }
    }

    private void CreateRoomDownwards()
    {
        downCounter++;

        if (transform.position.y > minY)
        {
            var roomDetector = Physics2D.OverlapCircle(transform.position, 1, roomLayerMask);
            var room = roomDetector.GetComponent<RoomProperties>();

            if (room.Type != RoomType.LeftRightBottom && room.Type != RoomType.LeftRightTopBottom)
            {
                if (downCounter >= 2)
                {
                    room.RoomDestruction();
                    CreateRoomAtCurrentPosition(RoomType.LeftRightTopBottom);
                }
                else
                {
                    room.RoomDestruction();
                    CreateRoomAtCurrentPosition(RoomType.LeftRightBottom, RoomType.LeftRightTopBottom);
                }
            }

            UpdatePositionAndCreateRoom(-moveAmount, 1, 6, true, RoomType.LeftRightTopBottom, RoomType.LeftRightTop);
        }
        else
        {
            StopGeneration = true;
        }
    }

    private void UpdatePositionAndCreateRoom(float amountToMove, int directionMinRange, int directionMaxRange, bool moveOnY, params RoomType[] roomType)
    {
        var newPos = !moveOnY ? new Vector2(transform.position.x + amountToMove, transform.position.y) :
                                new Vector2(transform.position.x, transform.position.y + amountToMove);
             
        transform.position = newPos;
        direction = Random.Range(directionMinRange, directionMaxRange);

        CreateRoomAtCurrentPosition(roomType);
    }

    private void CreateRoomAtCurrentPosition(params RoomType[] roomType)
    {
        var roomIndex = 0;

        if (!roomType.Contains(RoomType.Any))
        {
            var roomQuery = Rooms.Where(room => roomType.Contains(room.GetComponent<RoomProperties>().Type)).ToList();
            roomIndex = Random.Range(0, roomQuery.Count);
            createdRooms.Add(Instantiate(roomQuery[roomIndex], transform.position, Quaternion.identity));
        }
        else
        {
            roomIndex = Random.Range(0, Rooms.Length);
            createdRooms.Add(Instantiate(Rooms[roomIndex], transform.position, Quaternion.identity));
        }
    }
}