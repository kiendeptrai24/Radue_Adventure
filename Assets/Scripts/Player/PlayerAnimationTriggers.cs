using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationTriggers : MonoBehaviour
{
    private Player player => GetComponentInParent<Player>();
    private void AnimationTrigger()
    {
        player.AnimationTrigger();
    }
    private void AttackTriggers()
    {
        
        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.attackCheck.position,player.attackCheckRadius);
        foreach(var hit in colliders)
        {
            if(hit.GetComponent<Enemy>() != null)
            {
                AudioManger.instance.PlayerSFX(0,null);
                EnemyStats _target = hit.GetComponent<EnemyStats>();
                if(_target != null)
                    player.stats.DoDamage(_target);

                Inventory.instance.GetEquipment(EquipmentType.Weapon)?.Effect(_target.transform);

            }
            else
                AudioManger.instance.PlayerSFX(2,null);

        }
    }
    private void ThrowSword()
    {
        AudioManger.instance.PlayerSFX(27,null);
        SkillManager.instance.sword.CreateSword();
    }
}
