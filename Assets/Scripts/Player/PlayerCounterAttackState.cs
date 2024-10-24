using UnityEngine;

public class PlayerCounterAttackState : PlayerState
{
    private bool canCreateClone;
    public PlayerCounterAttackState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }
    public override void Enter()
    {
        base.Enter();
        canCreateClone=true;
        stateTimer = player.counterAtackDuration;
        player.anim.SetBool("SuccessfullCounterAttack",false);
    }
    public override void Update()
    {
        base.Update();
        player.ZeroVelocity();

        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.attackCheck.position,player.attackCheckRadius);
        foreach(var hit in colliders)
        {
            if(hit.GetComponent<Arrow_Controller>() != null)
            {
                hit.GetComponent<Arrow_Controller>().FlipArrow();
                SuccessfullCounterAttack();
            }
                
            if(hit.GetComponent<Enemy>() != null)
            {
                if(hit.GetComponent<Enemy>().CanBeStunned())
                {
                    SuccessfullCounterAttack();
                    player.skill.parry.CanUseSkill(); //going to use to restore health on parry

                    if (canCreateClone)
                    {
                        canCreateClone = false;
                        player.skill.parry.MakeMirageOnParry(hit.transform);
                    }
                }
            }
        }
        if(stateTimer < 0 || triggerCalled)
            stateMachine.ChangeState(player.idleState);

    }

    private void SuccessfullCounterAttack()
    {
        stateTimer = 10;//any value bigger 1
        player.anim.SetBool("SuccessfullCounterAttack", true);
    }

    public override void Exit()
    {
        base.Exit();
        
    }
}
