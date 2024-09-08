using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonStunnedState : EnemyState
{
    private EnemySkeleton enemySkeleton;
    public SkeletonStunnedState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName,EnemySkeleton _enemySkeleton) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        enemySkeleton = _enemySkeleton;
    }
    public override void Enter()
    {
        base.Enter();
        
        enemySkeleton.fx.InvokeRepeating("RedColorBlink",0f,0.1f);
        stateTimer = enemySkeleton.stunDuration;
        myrb.velocity=new Vector2(-enemySkeleton.facingDir * enemySkeleton.stunDirection.x,enemySkeleton.stunDirection.y);


    }
    public override void Update() 
    {
        base.Update();
        if(stateTimer < 0)
            StateMachine.ChangeState(enemySkeleton.idleState);
    }
    public override void Exit()
    {
        base.Exit();
        enemySkeleton.fx.Invoke("CancelColorChange",0);
    }
}
