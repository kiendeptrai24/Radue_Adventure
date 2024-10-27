

public class PlayerDashState : PlayerState
{
    public PlayerDashState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }
    public override void Enter()
    {
        base.Enter();
        player.skill.dash.cloneOnDash();
        stateTimer=player.dashDuration;
        player.stats.MakeInvincible(true);
        cc.enabled=false;
    }
    public override void Update()
    {
        base.Update();
        if(player.IsWallDetected() && !player.IsGroundDetected())
            stateMachine.ChangeState(player.wallSlideState); 
        player.SetVelocity(player.dashSpeed * player.dashDir,0);
        if (stateTimer<0)
            stateMachine.ChangeState(player.idleState);
        player.fx.CreateAfterImage();

    }
    public override void Exit()
    {
        base.Exit();
        player.SetVelocity(0,myrb.velocity.y);
        player.skill.dash.CloneOnArrival();
        player.stats.MakeInvincible(false);
        cc.enabled=true;



    }
}
