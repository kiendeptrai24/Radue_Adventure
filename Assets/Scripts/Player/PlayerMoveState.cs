using System.Collections;
using System.Collections.Generic;
using System.Xml.XPath;
using UnityEngine;

public class PlayerMoveState : PlayerGroundedState
{
    public PlayerMoveState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }
    public override void Enter()
    {
        base.Enter();
        AudioManger.instance.PlayerSFX(14,null);
    }
    public override void Update()
    {
        base.Update();
        player.SetVelocity(xInput * player.moveSpeed,myrb.velocity.y);
        if(xInput == 0 || player.IsWallDetected())
            stateMachine.ChangeState(player.idleState);
        
    }
    public override void Exit()
    {
        base.Exit();
        AudioManger.instance.StopSFX(14);

    }
}
