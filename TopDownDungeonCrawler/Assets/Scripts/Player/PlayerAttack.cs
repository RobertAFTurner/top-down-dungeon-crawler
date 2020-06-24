using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField]
    private LayerMask enemyLayerMask;

    [SerializeField]
    Transform[] attackPoints;

    [SerializeField]
    float attackRadius;

    [SerializeField]
    Camera mainCamera;

    [SerializeField]
    Animator animator;

    void Start()
    {
        
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
            Attack();
    }

    private void Attack()
    {
        var hitEnemies = AttackByAngle();

        if (hitEnemies != null && hitEnemies.Any())
            hitEnemies.ToList().ForEach(enemy => enemy.GetComponent<EnemyController>().TakeDamage(5));
    }

    private Collider2D[] AttackByAngle()
    {
        var mousePos = Input.mousePosition;
        var direction = MousePosition.GetMouseWorldPosition(mousePos, mainCamera);
        var attackDirection = (transform.position - direction).normalized;
        var angle = Mathf.Atan2(attackDirection.y, attackDirection.x) * Mathf.Rad2Deg;

        Collider2D[] hitEnemies = null;

        if ((angle < -135 && angle > -180) || (angle > 135 && angle < 180))
        {
            hitEnemies = Physics2D.OverlapCircleAll(attackPoints[0].transform.position, attackRadius, enemyLayerMask);
            //animator.SetTrigger("AttackRight");
        }
        else if (angle > -45 && angle < 45)
        {
            hitEnemies = Physics2D.OverlapCircleAll(attackPoints[1].transform.position, attackRadius, enemyLayerMask);
            //animator.SetTrigger("AttackLeft");
        }
        else if (angle < -45 && angle > -135)
        {
            hitEnemies = Physics2D.OverlapCircleAll(attackPoints[2].transform.position, attackRadius, enemyLayerMask);
            //animator.SetTrigger("AttackUp");
        }
        else if (angle > 45 && angle < 135)
        {
            hitEnemies = Physics2D.OverlapCircleAll(attackPoints[3].transform.position, attackRadius, enemyLayerMask);
            //animator.SetTrigger("AttackDown");
        }

        return hitEnemies;
    }

    private void OnDrawGizmosSelected()
    {
        attackPoints.ToList().ForEach(attackPoint => Gizmos.DrawWireSphere(attackPoint.transform.position, attackRadius));        
    }
}
