using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAirState : PlayerState
{
    public PlayerAirState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {       
    }
    
    public override void Enter()
    {
        base.Enter();
    }
    public override void Update()
    {
        base.Update();
        player.SetVelocity(xInput * player.moveSpeed,myrb.velocity.y);
        if(player.IsWallDetected())
            stateMachine.ChangeState(player.wallSlideState);
        
        if(player.IsGroundDetected())
            stateMachine.ChangeState(player.idleState);
        
    }
    public override void Exit()
    {
        base.Exit();
    }

}
