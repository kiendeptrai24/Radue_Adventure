using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;


[CreateAssetMenu(fileName = "Heal effect", menuName = "Data/Item effect/Heal effect")]
public class Heal_Effect : Item_Effect
{
    [Range(0f,1f)]
    [SerializeField] private float healPerscent;
    public override void ExecuteEffect(Transform _enemyPosition)
    {
        
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();    

        int healAmount = Mathf.RoundToInt(playerStats.GetMaxHealthValue() * healPerscent);
        playerStats.IncreaseHealthBy(healAmount);

    }

}
