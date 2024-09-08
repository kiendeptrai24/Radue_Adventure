using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallSlideState : PlayerState
{
    public PlayerWallSlideState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }
    public override void Enter()
    {
        base.Enter();
    }
    public override void Update()
    {
        base.Update();
        if(Input.GetKeyDown(KeyCode.Space))
        {
            stateMachine.ChangeState(player.WallJumpState);
            return;
        }
        if(!player.IsWallDetected() && !player.IsGroundDetected())
            stateMachine.ChangeState(player.airState);
        if(xInput != 0 && player.facingDir != xInput || player.IsGroundDetected())
            stateMachine.ChangeState(player.idleState);
        if(yInput < 0)
            myrb.velocity =new Vector2(0, myrb.velocity.y);
        else
            myrb.velocity= new Vector2(0, myrb.velocity.y * 0.7f);
    }
    public override void Exit()
    {
        base.Exit();
    }
    

}
