using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;


public class PlayerState
{
    protected PlayerStateMachine stateMachine;
    protected Player player;
    protected Rigidbody2D myrb;
    protected CapsuleCollider2D cc;
    protected float xInput;
    protected float yInput;
    private string animBoolName;


    protected float stateTimer;
    protected bool triggerCalled;

    public PlayerState(Player _player,PlayerStateMachine _stateMachine,string _animBoolName)
    {
        this.player=_player;
        this.stateMachine=_stateMachine;
        this.animBoolName=_animBoolName;
    }
    public virtual void Enter()
    {
        player.anim.SetBool(animBoolName,true);
        myrb=player.rb;
        cc=player.GetComponent<CapsuleCollider2D>();
        triggerCalled = false;
    }
    public virtual void Update()
    {
        stateTimer -= Time.deltaTime;
        xInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetAxisRaw("Vertical");
        player.anim.SetFloat("yVelocity",myrb.velocity.y);

    }
    public virtual void Exit()
    {
        player.anim.SetBool(animBoolName,false);
    }
    public virtual void AnimationFinishTrigger()
    {
        triggerCalled=true;
    }
}
