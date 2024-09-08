using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Freeze enemies effect", menuName = "Data/Item effect/Freeze enemies")]
public class FreezeEnemies_Effect : Item_Effect
{
    [SerializeField] private float duration;

    public override void ExecuteEffect(Transform _transform)
    {
        PlayerStats playerStats =PlayerManager.instance.player.GetComponent<PlayerStats>();

        if(playerStats.currentHealth > playerStats.GetMaxHealthValue() * .2f)
        
        if(!Inventory.instance.CanUseArmor())
            return;
            
        Collider2D[] colliders = Physics2D.OverlapCircleAll(_transform.position,2);
        foreach(var hit in colliders)
        {
            hit.GetComponent<Enemy>()?.FreezeTimefor(duration);         
        }
    }
}
