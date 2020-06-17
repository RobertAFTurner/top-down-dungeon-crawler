using UnityEngine;

public class LevelGeneration : MonoBehaviour
{
    [SerializeField]
    private Transform[] startingPostions;

    [SerializeField]
    public GameObject[] Rooms; // index 0 --> LR, index 1 --> LRB, index 2 --> LRT, index 3 --> LRBT 

    [SerializeField]
    private LayerMask roomLayerMask;

    public bool StopGeneration { get; private set; } = false;

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
        Instantiate(Rooms[0], transform.position, Quaternion.identity);
        direction = Random.Range(1, 6);
    }

    private void Update()
    {
        if(!StopGeneration)
            Move();
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
            UpdatePositionAndCreateRoom(-moveAmount, 3, 6, 0, Rooms.Length);
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
            UpdatePositionAndCreateRoom(moveAmount, 1, 6, 0, Rooms.Length);

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
            var room = roomDetector.GetComponent<RoomType>();

            if (room.Type != 1 && room.Type != 3)
            {
                if (downCounter >= 2)
                {
                    room.RoomDestruction();
                    Instantiate(Rooms[3], transform.position, Quaternion.identity);
                }
                else
                {
                    room.RoomDestruction();
                    var bottomRoomIndex = Random.Range(1, 4);
                    bottomRoomIndex = bottomRoomIndex == 2 ? 1 : bottomRoomIndex;
                    Instantiate(Rooms[bottomRoomIndex], transform.position, Quaternion.identity);
                }
            }

            UpdatePositionAndCreateRoom(-moveAmount, 1, 6, 3, 4, true);
        }
        else
        {
            StopGeneration = true;
        }
    }

    private void UpdatePositionAndCreateRoom(float amountToMove, int directionMinRange, int directionMaxRange, int roomIndexMin, int roomIndexMax, bool moveOnY = false)
    {
        var newPos = !moveOnY ? new Vector2(transform.position.x + amountToMove, transform.position.y) :
                                new Vector2(transform.position.x, transform.position.y + amountToMove);
             
        transform.position = newPos;
        direction = Random.Range(directionMinRange, directionMaxRange);

        var roomIndex = Random.Range(roomIndexMin, roomIndexMax);
        Instantiate(Rooms[roomIndex], transform.position, Quaternion.identity);
    }
}