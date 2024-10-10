using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonIdleState : SkeletonGroundedState
{
    public SkeletonIdleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, EnemySkeleton _enemySkeleton) : base(_enemyBase, _stateMachine, _animBoolName, _enemySkeleton)
    {
    }

    public override void Enter()
    {
        base.Enter();
        stateTimer = enemySkeleton.idleTime;

    }
    public override void Update() 
    {
        base.Update();
        if(stateTimer < 0)
            StateMachine.ChangeState(enemySkeleton.moveState);
    }
    public override void Exit()
    {
        base.Exit();
        AudioManger.instance.PlayerSFX(24,enemySkeleton.transform);
    }

}
   

