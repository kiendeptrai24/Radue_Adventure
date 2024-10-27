using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBringerSpellCastState : EnemyState
{
    private Enemy_DeathBringer enemy;
    private int AmountOfSpells;
    private float spellTimer;

    public DeathBringerSpellCastState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName,Enemy_DeathBringer _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        enemy = _enemy;
    }
    public override void Enter()
    {
        base.Enter();
        enemy.stats.MakeInvincible(true);
        AmountOfSpells = enemy.AmountOfSpells;
        spellTimer = .5f;
    }
    public override void Update()
    {
        base.Update();
        spellTimer -= Time.deltaTime;
        if(CanCast())
        {
            enemy.CastSpell();
        }
        if(AmountOfSpells <= 0)
            StateMachine.ChangeState(enemy.teleportState);
        
    }
    public override void Exit()
    {
        base.Exit();
        enemy.lastTimeCast = Time.time;
        enemy.stats.MakeInvincible(false);


    }
    private bool CanCast()
    {
        if(AmountOfSpells > 0 && spellTimer < 0)
        {
            AmountOfSpells--;
            spellTimer = enemy.spellCooldown;
            return true;
        }
        return false;
    }
}
