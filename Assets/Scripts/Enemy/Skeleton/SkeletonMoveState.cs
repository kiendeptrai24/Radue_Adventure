using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonMoveState : SkeletonGroundedState
{

    public SkeletonMoveState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, enemy_Skeleton _enemySkeleton) : base(_enemyBase, _stateMachine, _animBoolName, _enemySkeleton)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }
    public override void Update() 
    {
        base.Update();
        enemy.SetVelocity(enemy.moveSpeed * enemy.facingDir,enemy.rb.velocity.y);    
        if(enemy.IsWallDetected() || !enemy.IsGroundDetected())
        {
            enemy.Flip();
            StateMachine.ChangeState(enemy.idleState);
        }


    }
    public override void Exit()
    {
        base.Exit();

    }
}
