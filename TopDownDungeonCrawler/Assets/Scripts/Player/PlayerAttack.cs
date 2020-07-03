using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField]
    Transform[] attackPoints;

    public Transform currentAttackPoint;

    public float attackRadius;

    [SerializeField]
    Camera mainCamera;

    [SerializeField]
    Animator animator;

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
            TriggerAttack();
    }

    private void TriggerAttack()
    {
        var mousePos = Input.mousePosition;
        var direction = MousePosition.GetMouseWorldPosition(mousePos, mainCamera);
        var attackDirection = (transform.position - direction).normalized;
        var angle = Mathf.Atan2(attackDirection.y, attackDirection.x) * Mathf.Rad2Deg;

        if ((angle < -135 && angle > -180) || (angle > 135 && angle < 180))
        {
            animator.SetTrigger("Attack_S");
            currentAttackPoint = attackPoints[0];
        }
        else if (angle > -45 && angle < 45)
        {
            animator.SetTrigger("Attack_S");
            currentAttackPoint = attackPoints[1];
        }
        else if (angle < -45 && angle > -135)
        {
            currentAttackPoint = attackPoints[2];
            //animator.SetTrigger("AttackUp");
        }
        else if (angle > 45 && angle < 135)
        {
            currentAttackPoint = attackPoints[3];
            //animator.SetTrigger("AttackDown");
        }
    }

    private void OnDrawGizmosSelected()
    {
        attackPoints.ToList().ForEach(attackPoint => Gizmos.DrawWireSphere(attackPoint.transform.position, attackRadius));        
    }
}
