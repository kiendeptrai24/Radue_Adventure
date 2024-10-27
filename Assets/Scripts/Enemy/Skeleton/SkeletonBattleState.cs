
using UnityEngine;

public class SkeletonBattleState : EnemyState
{
    private Transform player;
    private enemy_Skeleton enemy;

    private int moveDir;
    private bool flippedOne;
    public SkeletonBattleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName,enemy_Skeleton _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        enemy = _enemy;
    }
    public override void Enter()
    {
        base.Enter();
        player= PlayerManager.instance.player.transform;
        if(player.GetComponent<PlayerStats>().isDead)
            StateMachine.ChangeState(enemy.moveState);
        flippedOne=false;
        stateTimer = enemy.battleTime;
        
    }
    public override void Update() 
    {
        base.Update();
        enemy.anim.SetFloat("xVelocity", enemy.rb.velocity.x);
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
            if(flippedOne == false)
            {
                flippedOne = true;
                enemy.Flip();
            }
            if(stateTimer < 0 || Vector2.Distance(player.transform.position, enemy.transform.position) > 7)
                StateMachine.ChangeState(enemy.idleState);
        }
        float distanceToPlayerX = Mathf.Abs(player.position.x - enemy.transform.position.x);
        if(distanceToPlayerX < 2)
            return;
        if(player.position.x >enemy.transform.position.x)
        {
            moveDir = 1;
        }
        else if(player.position.x < enemy.transform.position.x)
        {
            moveDir =-1;
        }
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
