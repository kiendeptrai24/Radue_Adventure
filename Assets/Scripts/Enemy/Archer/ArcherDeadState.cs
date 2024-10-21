using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ArcherDeadState : EnemyState
{
    private Enemy_Archer enemy;

    public ArcherDeadState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName,Enemy_Archer _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        
        enemy.anim.SetBool(enemy.lastAnimBoolName, false);
        enemy.anim.speed = 0;
        enemy.cd.enabled = false;

        stateTimer = .15f;
        rb.velocity=new Vector2(7,10);
    }
    public override void Update()
    {
        base.Update();
        if(stateTimer>0)
            rb.velocity = new Vector2(0,10);
            

    }
    public override void Exit()
    {
        base.Exit();

    }
}