using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBringerBattleState : EnemyState
{
    private Transform player;
    private Enemy_DeathBringer enemy;
    private int moveDir;
    private float waitingforteleport;
    private bool flippedOne;

    public DeathBringerBattleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName,Enemy_DeathBringer _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        player= PlayerManager.instance.player.transform;
        // if(player.GetComponent<PlayerStats>().isDead)
        //     StateMachine.ChangeState(enemy.moveState);
        waitingforteleport =10;
        flippedOne=false;

    }
    public override void Update() 
    {
        base.Update();
        waitingforteleport -= Time.deltaTime;
        Debug.Log(waitingforteleport);
        if(waitingforteleport < 0)
            StateMachine.ChangeState(enemy.teleportState);
        if(enemy.IsPlayerDetected())
        {
            stateTimer = enemy.battleTime;
            if(enemy.IsPlayerDetected().distance < enemy.attackDistance)
            {              
                if(CanAttack())
                {
                    
                    StateMachine.ChangeState(enemy.attackState);                    
                }
                else
                    StateMachine.ChangeState(enemy.idleState);
            }

        }
        else{
            if(flippedOne == false)
            {
                flippedOne = true;
                enemy.Flip();
            }
        }
        float distanceToPlayerX = Mathf.Abs(player.position.x - enemy.transform.position.x);
        if(distanceToPlayerX < 2)
            return;

        if(player.position.x > enemy.transform.position.x)
        {
            moveDir = 1;
        }
        else if(player.position.x < enemy.transform.position.x)
        {
            moveDir =-1;
        }
        if(enemy.IsPlayerDetected() && enemy.IsPlayerDetected().distance < enemy.attackDistance - .1f)
            return;
        
        enemy.SetVelocity(enemy.moveSpeed * moveDir,rb.velocity.y);
    }
    public override void Exit()
    {
        base.Exit();

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