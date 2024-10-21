using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedState : PlayerState
{
    public PlayerGroundedState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }
    public override void Update()
    {
        base.Update();
        
        if(Input.GetKeyDown(KeyCode.R) && player.skill.blackhole.blackholeUnlocked && player.skill.blackhole.cooldownTimer < 0)
            stateMachine.ChangeState(player.blackholeState);
        else if(Input.GetKeyDown(KeyCode.R) && player.skill.blackhole.blackholeUnlocked && player.skill.blackhole.cooldownTimer > 0)
            player.fx.CreatePupUpText("Cooldown",Color.gray);

        if(Input.GetKeyDown(KeyCode.Mouse1) && HasNoSword() && player.skill.sword.swordUnlocked)
            stateMachine.ChangeState(player.aimSwordState);
        if(Input.GetKeyDown(KeyCode.Q) && player.skill.parry.parryUnlocked && player.skill.parry.cooldownTimer < 0)
            stateMachine.ChangeState(player.counterAttackState);
        if(Input.GetKeyDown(KeyCode.Mouse0))
            stateMachine.ChangeState(player.primaryAttackState);
        if(!player.IsGroundDetected())
            stateMachine.ChangeState(player.airState);
        if(Input.GetKeyDown(KeyCode.Space) && player.IsGroundDetected())
            stateMachine.ChangeState(player.jumpState);
    }
    private bool HasNoSword()
    {
        if(!player.sword)
            return true;
        player.sword.GetComponent<Sword_Skill_Controller>().ReturnSword();
        return false;
    }
    public override void Exit()
    {
        base.Exit();
    }
}
