using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonStunnedState : EnemyState
{
    private enemy_Skeleton enemy;
    public SkeletonStunnedState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName,enemy_Skeleton _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        enemy = _enemy;
    }
    public override void Enter()
    {
        base.Enter();
        
        enemy.fx.InvokeRepeating("RedColorBlink",0f,0.1f);
        stateTimer = enemy.stunDuration;
        rb.velocity=new Vector2(-enemy.facingDir * enemy.stunDirection.x,enemy.stunDirection.y);


    }
    public override void Update() 
    {
        base.Update();
        if(stateTimer < 0)
            StateMachine.ChangeState(enemy.idleState);
    }
    public override void Exit()
    {
        base.Exit();
        enemy.fx.Invoke("CancelColorChange",0);
    }
}
