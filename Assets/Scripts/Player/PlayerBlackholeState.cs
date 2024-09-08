using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Callbacks;
using UnityEngine;

public class PlayerBlackholeState : PlayerState
{
    private float flyTime = .4f;
    private bool skillUsed;
    private float defaultGrabity;

    public PlayerBlackholeState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }
    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
    }
    public override void Enter()
    {
        base.Enter();
        defaultGrabity = player.myrb.gravityScale;
        skillUsed = false;
        stateTimer = flyTime;
        myrb.gravityScale = 0;
        cc.enabled = false;
       
    }
    public override void Update()
    {
        base.Update();
        if(stateTimer>0)
            myrb.velocity = new Vector2(0,15);
        if(stateTimer<0)
        {
            myrb.velocity =new Vector2(0,-.1f);
            if(!skillUsed)
            {
                if(player.skill.blackhole.CanUseSkill())
                    skillUsed=true;
            }
        }
        //We exit state in blackhole skills controller when all of the attacks are over
        if(player.skill.blackhole.SkillCompleted())
            stateMachine.ChangeState(player.airState);
    }
    public override void Exit()
    {
        base.Exit();
        player.fx.MakeTransprent(false);
        player.myrb.gravityScale = defaultGrabity;
        cc.enabled = true;

    }

}
