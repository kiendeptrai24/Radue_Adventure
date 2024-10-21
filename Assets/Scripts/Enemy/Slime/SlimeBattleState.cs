
using UnityEngine;

public class SlimeBattleState : EnemyState
{
    private Enemy_Slime enemy;
    private Transform player;
    private int moveDir;
    public SlimeBattleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName,Enemy_Slime _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        enemy = _enemy;
    }
    public override void Enter()
    {
        base.Enter();
        player= PlayerManager.instance.player.transform;
        if(player.GetComponent<PlayerStats>().isDead)
            StateMachine.ChangeState(enemy.moveState);
    }
    public override void Update() 
    {
        base.Update();
        if(enemy.IsPlayerDetected())
        {
            stateTimer = enemy.battleTime;
            if(enemy.IsPlayerDetected().distance < enemy.attackDistance)
            {              
                if(CanAttack())
                {
                    StateMachine.ChangeState(enemy.attackState);                    
                }
            }
        }
        else
        {

            StateMachine.ChangeState(enemy.idleState);
        }

        if(player.position.x >enemy.transform.position.x)
        {
            moveDir = 1;
        }
        else if(player.position.x < enemy.transform.position.x)
        {
            moveDir =-1;
        }
        if(enemy.IsPlayerDetected()  && enemy.IsPlayerDetected().distance < enemy.attackDistance - .1f)
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

