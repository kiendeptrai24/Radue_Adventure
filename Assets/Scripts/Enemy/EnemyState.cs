using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState 
{
    protected EnemyStateMachine StateMachine;
    protected Enemy enemyBase;
    protected Rigidbody2D rb;

    private string animBoolName;

    protected float stateTimer;
    protected bool triggerCalled;
    

    public EnemyState(Enemy _enemyBase,EnemyStateMachine _stateMachine,string _animBoolName)
    {
        enemyBase = _enemyBase;
        animBoolName = _animBoolName;
        StateMachine = _stateMachine;
    }
    public virtual void Enter()
    {
        rb=enemyBase.rb;
        triggerCalled = false;
        enemyBase.anim.SetBool(animBoolName,true);
    }
    public virtual void Update() 
    {
        stateTimer -= Time.deltaTime;

    }
    public virtual void Exit()
    {
        enemyBase.anim.SetBool(animBoolName,false);
        enemyBase.ZeroVelocity();
        enemyBase.AssignLastAnimName(animBoolName);
    }
    public virtual void AnimationFinishTrigger()
    {
        Debug.Log("True");
        triggerCalled = true;
    } 
    
}
