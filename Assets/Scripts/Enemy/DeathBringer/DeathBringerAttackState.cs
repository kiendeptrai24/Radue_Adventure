using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBringerAttackState : EnemyState
{
    private Enemy_DeathBringer enemy;
    public DeathBringerAttackState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName,Enemy_DeathBringer _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        enemy = _enemy;
    }
    public override void Enter()
    {
        base.Enter();
        enemy.chanceToTeleport += 5;

    }
    public override void Update()
    {
        base.Update();
        enemy.ZeroVelocity();
        if(triggerCalled)
        {
            if(enemy.CanTeleport())
                StateMachine.ChangeState(enemy.teleportState);
            else
                StateMachine.ChangeState(enemy.battleState);
        }
            

    }
    public override void Exit()
    {
        base.Exit();
        enemy.lastTimeAttacked = Time.time;
    }
}