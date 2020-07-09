
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

    [SerializeField]
    private Animator animator;

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
        var attackDirection = (transform.position - player.transform.position).normalized;
        var angle = Mathf.Atan2(attackDirection.y, attackDirection.x) * Mathf.Rad2Deg;

        animator.SetBool("Move_Left", false);
        animator.SetBool("Move_Right", false);
        animator.SetBool("Move_Up", false);
        animator.SetBool("Move_Down", false);

        if ((angle < -135 && angle > -180) || (angle > 135 && angle < 180))
        {        
            animator.SetBool("Move_Right", true);
        }
        else if (angle > -45 && angle < 45)
        {
            animator.SetBool("Move_Left", true);
        }
        else if (angle < -45 && angle > -135)
        {
            animator.SetBool("Move_Up", true);
        }
        else if (angle > 45 && angle < 135)
        {
            animator.SetBool("Move_Down", true);
        }

        if (distance < retreatRange)
        {
            MoveAway();
            Attack(angle);
        }

        else if (distance > retreatRange && distance < attackRange)
        {
            Attack(angle);
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

    private void Attack(float angle)
    { 
        if (attackTimer < 0)
        {
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
        Gizmos.DrawLine(transform.position, player.transform.position);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, retreatRange);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, engageRange);
    }
}
