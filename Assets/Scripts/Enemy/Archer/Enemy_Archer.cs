using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

#if UNITY_EDITOR
using UnityEditor;
#endif
public class Enemy_Archer : Enemy
{
    [Header("Archer spicific info")]
    [SerializeField] private GameObject arrowPrefabs;
    [SerializeField] private float arrowSpeed;
    public Vector2 jumpVelocity;
    public float JumpCooldown;
    public float safeDistance; // how close player should be to trigger jump on battle state
    [HideInInspector] public float lastTimeJumped;
    [Header("Additinonal collision check")]
    [SerializeField]  private Transform groundBehindCheck;
    [SerializeField] private Vector2 groundBehindSize;

    #region States
    public ArcherIdleState idleState { get;private set; }
    public ArcherMoveState moveState { get;private set; }
    public ArcherBattleState battleState { get;private set; }
    public ArcherAttackState attackState { get;private set; }
    public ArcherStunnedState stunnedState{ get;private set; }
    public ArcherDeadState deadState { get;private set; }
    public ArcherJumpState jumpState { get;private set; }

    #endregion
    protected override void Reset()
    {
        base.Reset();
        jumpVelocity = new Vector2(10,15);
        JumpCooldown = 4;
        safeDistance = 3;
        #if UNITY_EDITOR
        if(arrowPrefabs == null)
            arrowPrefabs = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Controller/Arrow_Controller.prefab");
        #endif
        

    }

    protected override void Awake()
    {
        base.Awake();
        idleState = new ArcherIdleState(this, stateMachine,"Idle",this);
        moveState = new ArcherMoveState(this, stateMachine,"Move",this);
        battleState = new ArcherBattleState(this, stateMachine,"Idle",this);
        attackState = new ArcherAttackState(this,stateMachine,"Attack",this);
        stunnedState = new ArcherStunnedState(this, stateMachine,"Stunned",this);
        deadState = new ArcherDeadState(this, stateMachine,"Move",this);
        jumpState = new ArcherJumpState(this, stateMachine,"Jump",this);
    }
    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(idleState);
    }
    public override bool CanBeStunned()
    {
        if(base.CanBeStunned())
        {
            stateMachine.ChangeState(stunnedState);
            return true;
        }
        return false;
    }
    public override void Die()
    {
        base.Die();
        stateMachine.ChangeState(deadState);
    }
    public override void AnimationSpecialAttackTrigger()
    {
        GameObject newArrow = Instantiate(arrowPrefabs,attackCheck.transform.position,quaternion.identity);
        newArrow.GetComponent<Arrow_Controller>().SetupArrow(arrowSpeed * facingDir,stats);

    }
    public bool GroundBehind() => Physics2D.BoxCast(groundBehindCheck.position,groundBehindSize,0 ,Vector2.zero,0 ,whatIsGround);
    public bool WallBehind() => Physics2D.Raycast(wallCheck.position,Vector2.right * -facingDir, wallCheckDistance + 2, whatIsGround);
    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.DrawWireCube(groundBehindCheck.position,groundBehindSize);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + (wallCheckDistance + 2) * -facingDir,wallCheck.position.y));
        Gizmos.color = Color.gray;
        Gizmos.DrawLine(new Vector3(transform.position.x,transform.position.y +.3f), new Vector3((transform.position.x + safeDistance)* facingDir,transform.position.y+.3f));
    }
}
