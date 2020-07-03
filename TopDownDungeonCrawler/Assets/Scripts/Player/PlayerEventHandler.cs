using System.Linq;
using UnityEngine;

public class PlayerEventHandler : MonoBehaviour
{
    [SerializeField]
    private LayerMask enemyLayerMask;

    [SerializeField]
    PlayerAttack attackController;

    public void AttackEvent()
    {        
        var hitEnemies = Physics2D.OverlapCircleAll(attackController.currentAttackPoint.transform.position, attackController.attackRadius, enemyLayerMask);

        if (hitEnemies != null && hitEnemies.Any())
            hitEnemies.ToList().ForEach(enemy => enemy.GetComponent<EnemyController>().TakeDamage(5));
    }
}
