using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeMoveState : SlimeGroundState
{
    
    public SlimeMoveState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Slime _enemy) : base(_enemyBase, _stateMachine, _animBoolName, _enemy)
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