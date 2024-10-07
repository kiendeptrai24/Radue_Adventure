
using UnityEngine;

public class PlayerJumpState : PlayerState
{
    public PlayerJumpState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }
    public override void Enter()
    {
        base.Enter();
        myrb.velocity = new Vector2(myrb.velocity.x,player.jumpForce);
    }
    public override void Update()
    {
        base.Update();
        if(myrb.velocity.y  < 0)
            stateMachine.ChangeState(player.airState);
        player.SetVelocity(xInput * player.moveSpeed,myrb.velocity.y);
        
    }
    public override void Exit()
    {
        base.Exit();
    }

}
