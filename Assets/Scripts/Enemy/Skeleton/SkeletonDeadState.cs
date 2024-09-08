using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonDeadState : EnemyState
{
    private EnemySkeleton enemySkeleton;
    public SkeletonDeadState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName,EnemySkeleton _enemySkeleton) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        enemySkeleton = _enemySkeleton;
    }
    public override void Enter()
    {
        base.Enter();
        
        enemySkeleton.anim.SetBool(enemySkeleton.lastAnimBoolName, false);
        enemySkeleton.anim.speed = 0;
        enemySkeleton.cd.enabled = false;

        stateTimer = .15f;
        myrb.velocity=new Vector2(7,10);
    }
    public override void Update()
    {
        base.Update();
        if(stateTimer>0)
            myrb.velocity = new Vector2(0,10);
            

    }
    public override void Exit()
    {
        base.Exit();

    }
}
