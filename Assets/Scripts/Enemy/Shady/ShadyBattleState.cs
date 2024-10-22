using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadyBattleState : EnemyState
{
    private Transform player;
    private Enemy_Shady enemy;
    private int moveDir;

    private float defaultSpeed;

    public ShadyBattleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName,Enemy_Shady _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        defaultSpeed = enemy.moveSpeed;
        enemy.moveSpeed = enemy.battleStateMoveSpeed;
        player= PlayerManager.instance.player.transform;
        if(player.GetComponent<PlayerStats>().isDead)
            StateMachine.ChangeState(enemy.moveState);
    }
    public override void Update()
    {
        base.Update();
        if (enemy.IsPlayerDetected())
        {
            stateTimer = enemy.battleTime;
            if (enemy.IsPlayerDetected().distance < enemy.attackDistance)
            {
                enemy.stats.KillEntity();
            }
        }
        else
        {
            if(stateTimer < 0 || Vector2.Distance(player.transform.position, enemy.transform.position) > 7)
                StateMachine.ChangeState(enemy.idleState);
        }

        BattleStateFlipControll();

        enemy.SetVelocity(enemy.moveSpeed * moveDir,rb.velocity.y);
    }


    public override void Exit()
    {
        base.Exit();
        enemy.moveSpeed =defaultSpeed;

    }
    private void BattleStateFlipControll()
    {
        if (player.position.x > enemy.transform.position.x && enemy.facingDir == -1)
            moveDir = 1;
        else if (player.position.x < enemy.transform.position.x && enemy.facingDir == 1)
            moveDir = -1;
    }
    private bool CanAttack()
    {
        if(Time.time >= enemy.lastTimeAttacked + enemy.attackCooldown)
        {
            enemy.attackCooldown = Random.Range(enemy.minAttackCooldown,enemy.maxAttackCooldown);
            enemy.lastTimeAttacked=Time.time;
            return true;
        }
        //attack is on cooldown
        
        return false;
    }

}