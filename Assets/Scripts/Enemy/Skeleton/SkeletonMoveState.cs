using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonMoveState : SkeletonGroundedState
{

    public SkeletonMoveState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, EnemySkeleton _enemySkeleton) : base(_enemyBase, _stateMachine, _animBoolName, _enemySkeleton)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }
    public override void Update() 
    {
        base.Update();
        enemySkeleton.SetVelocity(enemySkeleton.moveSpeed * enemySkeleton.facingDir,enemySkeleton.myrb.velocity.y);    
        if(enemySkeleton.IsWallDetected() || !enemySkeleton.IsGroundDetected())
        {
            enemySkeleton.Flip();
            StateMachine.ChangeState(enemySkeleton.idleState);
        }


    }
    public override void Exit()
    {
        base.Exit();

    }
}
