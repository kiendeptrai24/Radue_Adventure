using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBringerIdleState : EnemyState
{
    private Player player;
    private Enemy_DeathBringer enemy;

    public DeathBringerIdleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName,Enemy_DeathBringer _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        player = PlayerManager.instance.player;
        stateTimer = enemy.idleTime;

    }
    public override void Update() 
    {
        base.Update();
        if(Vector2.Distance(player.transform.position, enemy.transform.position) < 20)
            enemy.bossFightBegun = true;
        if(stateTimer <= 0 && enemy.bossFightBegun)
            StateMachine.ChangeState(enemy.battleState);
    }
    public override void Exit()
    {
        base.Exit();
        AudioManger.instance.PlayerSFX(24,enemy.transform);
    }

}