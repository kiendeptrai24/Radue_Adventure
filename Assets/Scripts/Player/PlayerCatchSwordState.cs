
using UnityEngine;

public class PlayerCatchSwordState : PlayerState
{
    private Transform sword;
    public PlayerCatchSwordState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }
    public override void Enter()
    {
        base.Enter();
        AudioManger.instance.PlayerSFX(28,null);

        sword =player.sword.transform;
        if(player.transform.position.x < sword.position.x && player.facingDir!=1)
            player.Flip();
        else if(player.transform.position.x > sword.position.x && player.facingDir==1)
            player.Flip();

        myrb.velocity = new Vector2(player.swordReturnImpact * -player.facingDir,myrb.velocity.y);
       
    }
    public override void Update()
    {
        base.Update();
        if(triggerCalled)
            stateMachine.ChangeState(player.idleState);
    }
    public override void Exit()
    {
        base.Exit();
        player.ZeroVelocity();
        player.StartCoroutine("BusyFor",.1f);
        player.hasSword = false;
        

    }
}
