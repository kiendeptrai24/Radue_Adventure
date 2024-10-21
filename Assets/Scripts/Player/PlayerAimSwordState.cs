using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAimSwordState : PlayerState
{
    public PlayerAimSwordState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        ObjectPooling.instance.DotsActive(true);
        player.hasSword = true;
    }
    public override void Update()
    {
        base.Update();
        player.ZeroVelocity();
        if(Input.GetKeyUp(KeyCode.Mouse1))
            stateMachine.ChangeState(player.idleState);

        Vector2 mousePosision= Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if(player.transform.position.x < mousePosision.x && player.facingDir!=1)
            player.Flip();
        else if(player.transform.position.x > mousePosision.x && player.facingDir==1)
            player.Flip();
    }
    public override void Exit()
    {
        base.Exit();
        player.StartCoroutine("BusyFor",.2f);
    }
}
