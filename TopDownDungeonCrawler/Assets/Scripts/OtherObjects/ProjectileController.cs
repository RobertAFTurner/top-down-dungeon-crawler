
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    private float projectileSpeed;
    private float projectileAngle;

    [SerializeField]
    Rigidbody2D rigidbody;

    void Start()
    {
        transform.eulerAngles = new Vector3(0, 0, projectileAngle);
        rigidbody.velocity = -transform.right * projectileSpeed;
    }

    public void Initialize(float angle, float speed)
    {
        projectileSpeed = speed;
        projectileAngle = angle;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Terrain")
            Destroy(gameObject);


        if (collision.tag == "Player")
        {
            collision.GetComponent<PlayerController>().TakeDamage(3);
            Destroy(gameObject);
        }
    }
}
