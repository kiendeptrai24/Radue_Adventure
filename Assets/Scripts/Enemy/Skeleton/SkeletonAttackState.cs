using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonAttackState : EnemyState
{
    private EnemySkeleton enemySkeleton;
    public SkeletonAttackState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName,EnemySkeleton _enemySkeleton) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        enemySkeleton = _enemySkeleton;
    }
    public override void Enter()
    {
        base.Enter();

    }
    public override void Update()
    {
        base.Update();
        enemySkeleton.ZeroVelocity();
        if(triggerCalled)
        {
            StateMachine.ChangeState(enemySkeleton.idleState);
        }
            

    }
    public override void Exit()
    {
        base.Exit();
        enemySkeleton.lastTimeAttacked = Time.time;
    }
}
