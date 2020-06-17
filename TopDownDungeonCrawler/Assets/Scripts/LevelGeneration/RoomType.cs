using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class RoomType : MonoBehaviour
{
    [SerializeField]
    private int roomType;

    public int Type => roomType;

    public void RoomDestruction()
    {
        Destroy(gameObject);
    }
}
