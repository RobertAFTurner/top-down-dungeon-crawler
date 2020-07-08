using System.Linq;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField]
    Transform[] attackPoints;

    [SerializeField]
    private Transform rendererTransform;

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
            rendererTransform.rotation = new Quaternion(0, 0, 0, 0);
        }
        else if (angle > -45 && angle < 45)
        {
            animator.SetTrigger("Attack_S");
            currentAttackPoint = attackPoints[1];
            rendererTransform.rotation = new Quaternion(0, 180, 0, 0);
        }
        else if (angle < -45 && angle > -135)
        {
            currentAttackPoint = attackPoints[2];
            animator.SetTrigger("Attack_U");
        }
        else if (angle > 45 && angle < 135)
        {
            currentAttackPoint = attackPoints[3];
            animator.SetTrigger("Attack_D");
        }
    }

    private void OnDrawGizmosSelected()
    {
        attackPoints.ToList().ForEach(attackPoint => Gizmos.DrawWireSphere(attackPoint.transform.position, attackRadius));        
    }
}
