
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.Timeline;

public class EnemyController : EntityController
{
    private int hitPoints = 10;
    private GameObject player;

    private float attackTimer;

    [SerializeField]
    private float attackRate;

    [SerializeField]
    private float speed;

    [SerializeField]
    private float attackRange;

    [SerializeField]
    private float engageRange;

    [SerializeField]
    private float retreatRange;

    [SerializeField]
    private GameObject projectile;

    public void Start()
    {
        attackTimer = attackRate;
        player = GameObject.Find("PlayerCharacter");
    }

    public void Update()
    {
        HandleMove();
        attackTimer -= Time.deltaTime;

        if (hitPoints <= 0)
            HandleDeath();
    }

    private void HandleMove()
    {
        var distance = Vector3.Distance(player.transform.position, transform.position);

        if (distance < retreatRange)
        {
            MoveAway();
            Attack();
        }

        else if (distance > retreatRange && distance < attackRange)
        {
            Attack();
        }
        else if (distance > attackRange && distance < engageRange)
        {
            MoveCloser();
        }
    }

    private void MoveCloser()
    {
        var step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, player.transform.position, step);
    }

    private void MoveAway()
    {
        var step = -1 * speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, player.transform.position, step);
    }

    private void Attack()
    {
        if (attackTimer < 0)
        {
            var attackDirection = (transform.position - player.transform.position).normalized;
            var angle = Mathf.Atan2(attackDirection.y, attackDirection.x) * Mathf.Rad2Deg;


            var instance = Instantiate(projectile, transform.position, Quaternion.identity);
            instance.GetComponent<ProjectileController>().Initialize(angle, 15);
            attackTimer = attackRate;
        }
    }

    public void TakeDamage(int damage)
    {
        hitPoints -= damage;
    }

    public override void HandleDeath()
    {
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, retreatRange);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, engageRange);
    }
}
