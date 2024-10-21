using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;



[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CapsuleCollider2D))]
[RequireComponent(typeof(EnemyStats))]
[RequireComponent(typeof(EntityFX))]
[RequireComponent(typeof(ItemDrop))]
public class Enemy : Entity
{
    [SerializeField] protected LayerMask whatIsPlayer;
    
    [Header("Stunned info")]
    public float stunDuration;
    public Vector2 stunDirection;
    protected bool canbeStunned;
    [SerializeField]  protected GameObject counterImage;

    [Header("Move info")]
    public float moveSpeed;
    public float idleTime;
    public float battleTime;
    public float defaultMoveSpeed;

    [Header("Attack info")]
    public float agroDistance = 2;
    public float attackDistance;
    public float attackCooldown;
    public float minAttackCooldown;
    public float maxAttackCooldown;
    [HideInInspector] public float lastTimeAttacked;


    public EnemyStateMachine stateMachine{ get; private set; }
    public string lastAnimBoolName { get; private set; }
    public EntityFX fx {get; private set;}
    protected override void Reset()
    {
        base.Reset();
        stunDuration = 1;
        stunDirection = new Vector2(10,12);
        defaultMoveSpeed=moveSpeed;
        moveSpeed = 1.2f;
        idleTime = 2;
        battleTime = 7;
        agroDistance = 2;
        attackDistance = 2;
        minAttackCooldown=1;
        maxAttackCooldown=2;
        whatIsPlayer =LayerMask.GetMask("Player");
         

    }

    protected override void Awake() {
        base.Awake();
        stateMachine = new EnemyStateMachine();



    }
    protected override void Start()
    {
        base.Start();
        fx=GetComponent<EntityFX>();

    }
    protected override void Update()
    {
        base.Update();
        stateMachine.currentState.Update();
        
    }
    public virtual void AssignLastAnimName(string _animBoolName) => lastAnimBoolName = _animBoolName;

    public override void SlowEntityBy(float _slowPercenttage, float _slowDuration)
    {
        moveSpeed = moveSpeed * (1 - _slowPercenttage);
        anim.speed = anim.speed * (1 - _slowPercenttage);
        Invoke("ReturnDefaultSpeed",_slowDuration);
    }
    protected override void ReturnDefaultSpeed()
    {
        base.ReturnDefaultSpeed();
        moveSpeed=defaultMoveSpeed;
    }


    public virtual void FreezeTime(bool _timeFrozen)
    {
        if(_timeFrozen)
        {
            moveSpeed = 0;
            anim.speed = 0;
        }
        else{
            moveSpeed =defaultMoveSpeed;
            anim.speed = 1;
        }
    }
    public virtual void AnimationSpecialAttackTrigger()
    {
    }
    public virtual void FreezeTimefor(float _duration) => StartCoroutine(FreezeTimerForCoroutine( _duration));
    protected virtual IEnumerator FreezeTimerForCoroutine(float _seconds)
    {
        FreezeTime(true);
        yield return new WaitForSeconds(_seconds);
        FreezeTime(false);
        
    }
    #region Counter Attack Window
    public virtual void OpenCounterAttackWindow()
    {
        canbeStunned = true;
        counterImage.SetActive(true);
    }
    public virtual void CloseCounterAttackWindow()
    {
        canbeStunned = false;
        counterImage.SetActive(false);
    }
        
    #endregion

    public virtual bool CanBeStunned()
    {
        if(canbeStunned)
        {
            CloseCounterAttackWindow();
            return true;
        }
        return false;
    }
    public virtual void AnimationFinishTrigger() => stateMachine.currentState.AnimationFinishTrigger();
    public virtual RaycastHit2D IsPlayerDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir,50,whatIsPlayer);
    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position,new Vector3(transform.position.x + attackDistance* facingDir,transform.position.y));
    }
}
