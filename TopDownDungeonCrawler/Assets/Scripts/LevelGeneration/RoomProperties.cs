using UnityEngine;

public enum RoomType
{
    Any = 0,
    LeftRight = 1,
    LeftRightBottom = 2,
    LeftRightTop = 3,
    LeftRightTopBottom = 4
}

public class RoomProperties : MonoBehaviour
{
    [SerializeField]
    private RoomType roomType;

    public RoomType Type => roomType;

    public void RoomDestruction()
    {
        Destroy(gameObject);
    }
}
