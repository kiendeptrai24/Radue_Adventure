using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySkeletonAnimationTriggers : MonoBehaviour
{
    private EnemySkeleton enemySkeleton=> GetComponentInParent<EnemySkeleton>();
    private void AnimationTrigger()
    {
        enemySkeleton.AnimationFinishTrigger();
    }
    private void AttackTriggers()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(enemySkeleton.attackCheck.position,enemySkeleton.attackCheckRadius);
        foreach (var hit in colliders)
        {
            if(hit.GetComponent<Player>() != null)
            {
                PlayerStats target = hit.GetComponent<PlayerStats>();
                enemySkeleton.stats.DoDamage(target);

            }
        }
    }
    private void OpenCounterWindow() => enemySkeleton.OpenCounterAttackWindow();
    private void CloseCounterWindow() => enemySkeleton.CloseCounterAttackWindow();
}
