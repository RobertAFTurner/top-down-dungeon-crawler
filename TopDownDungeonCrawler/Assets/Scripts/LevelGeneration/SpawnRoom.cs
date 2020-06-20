using UnityEngine;

public class SpawnRoom : MonoBehaviour
{
    [SerializeField]
    private LayerMask whatIsRoom;

    [SerializeField]
    private LevelGeneration levelGenerator;

    void Update()
    {
        if (levelGenerator.StopGeneration)
        {
            var roomDetector = Physics2D.OverlapCircle(transform.position, 1, whatIsRoom);
        
            if (roomDetector == null)
            {
                var roomIndex = Random.Range(0, levelGenerator.Rooms.Length);
                var instance = Instantiate(levelGenerator.Rooms[roomIndex], transform.position, Quaternion.identity);
                Destroy(instance.GetComponent<BoxCollider2D>());
            }
        
            Destroy(gameObject);
        }
    }
}