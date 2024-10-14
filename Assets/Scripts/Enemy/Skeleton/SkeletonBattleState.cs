
using UnityEngine;

public class SkeletonBattleState : EnemyState
{
    private Transform player;
    private EnemySkeleton enemy_Skeleton;

    private int moveDir;
    public SkeletonBattleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName,EnemySkeleton _enemy_Skeletone) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        enemy_Skeleton = _enemy_Skeletone;
    }
    public override void Enter()
    {
        base.Enter();
        player= PlayerManager.instance.player.transform;
        if(player.GetComponent<PlayerStats>().isDead)
            StateMachine.ChangeState(enemy_Skeleton.moveState);
    }
    public override void Update() 
    {
        base.Update();
        if(enemy_Skeleton.IsPlayerDetected())
        {
            stateTimer = enemy_Skeleton.battleTime;
            if(enemy_Skeleton.IsPlayerDetected().distance < enemy_Skeleton.attackDistance)
            {              
                if(CanAttack())
                {
                    StateMachine.ChangeState(enemy_Skeleton.attackState);                    
                }
            }
        }
        else
        {

            StateMachine.ChangeState(enemy_Skeleton.idleState);
        }

        if(player.position.x >enemy_Skeleton.transform.position.x)
        {
            moveDir = 1;
        }
        else if(player.position.x < enemy_Skeleton.transform.position.x)
        {
            moveDir =-1;
        }

        enemy_Skeleton.SetVelocity(enemy_Skeleton.moveSpeed * moveDir,myrb.velocity.y);
    }
    public override void Exit()
    {
        base.Exit();

    }
    private bool CanAttack()
    {
        if(Time.time >= enemy_Skeleton.lastTimeAttacked + enemy_Skeleton.attackCooldown)
        {
            enemy_Skeleton.attackCooldown = Random.Range(enemy_Skeleton.minAttackCooldown,enemy_Skeleton.maxAttackCooldown);
            enemy_Skeleton.lastTimeAttacked=Time.time;
            return true;
        }
        //attack is on cooldown
        
        return false;
    }

}
