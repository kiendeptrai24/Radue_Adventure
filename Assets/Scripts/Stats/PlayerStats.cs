using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : CharacterStats
{
    private Player player;
    protected override void Start()
    {
        base.Start();
        player = GetComponent<Player>();

    }
    public override void TakeDamage(int _damage)
    {
        base.TakeDamage(_damage);
    }
    protected override void Die()
    {
        base.Die();
        player.Die();
        GameManager.instance.lostCurrencyAmount = PlayerManager.instance.currency;
        PlayerManager.instance.currency = 0;
        
        GetComponent<PlayerItemDrop>()?.GenerateDrop();
    }
    protected override void DecreaseHealthBy(int _damage)
    {
        base.DecreaseHealthBy(_damage);
        if(isDead)
            return;
        if(_damage > GetMaxHealthValue() * .3f)
        {
            player.SetupKnockbackPower(new Vector2(10,5));
            player.fx.ScreenShake(player.fx.shakeHighDamage);
            int random = Random.Range(35, 37);
            AudioManger.instance.PlayerSFX(random,null);
        }
        ItemData_Equipment currentArmor = Inventory.instance.GetEquipment(EquipmentType.Armor);
        if(currentArmor != null)
            currentArmor.Effect(player.transform);
        
        
    }
    public override void OnEvasion()
    {
        player.skill.dodge.CreateMirageOnDodge();

    }
    public void CloneDoDamage(CharacterStats _targetStats, float _multiplier)
    {
         if (TargetCanAvoidAttack(_targetStats))
            return;
        int totalDamage = damage.GetValue() + strength.GetValue();
        if(_multiplier > 0)
            totalDamage = Mathf.RoundToInt(totalDamage * _multiplier);
        if(CanCrit())
        {
            totalDamage = CalculateCriticalDammage(totalDamage);
        }


        totalDamage = CheckTargetArmor(_targetStats, totalDamage);
        //call take damage of _targetStat Note (Dont have take damage of this object)
        _targetStats.TakeDamage(totalDamage);
        //if inventory current weapon has fire effect
        DoMagicalDamage(_targetStats);// remove if you dont want to apply magic hit on primary attack
    }
}
