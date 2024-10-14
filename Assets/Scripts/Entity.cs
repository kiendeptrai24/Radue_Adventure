using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public string nameOfChar;

    #region Components
    public Animator anim {get; private set;}
    public Rigidbody2D myrb {get; private set;}
    public SpriteRenderer sr {get; private set;}
    public CharacterStats stats {get; private set;}
    public CapsuleCollider2D cd {get; private set;}
    #endregion

    [Header("Knockback info")]
    [SerializeField] protected Vector2 knockbackPower;
    [SerializeField] protected float knockbackDuration;
    protected bool isKnocked;

    #region Collision
    [Header("Colllision info")]
    public Transform attackCheck;
    public float attackCheckRadius;
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected float groundCheckDistance;
    [Space]
    [SerializeField] protected Transform wallCheck;
    [SerializeField] protected float wallCheckDistance;
    [Space]
    [SerializeField] protected LayerMask whatIsGround;
    #endregion
    
    public System.Action onFlipped;
    public int facingDir{get; private set;}=1;
    protected bool facingRight=true;
    public int knockbackDir{get; private set;}

    protected virtual void Awake() 
    {
        
    }
    protected virtual void Start() 
    {
        
        sr=GetComponentInChildren<SpriteRenderer>();
        anim=GetComponentInChildren<Animator>();
        myrb=GetComponent<Rigidbody2D>();
        stats = GetComponent<CharacterStats>();
        cd=GetComponent<CapsuleCollider2D>();
    }
    protected virtual void Update()
    {
        
    }
    public virtual void SlowEntityBy(float _slowPercenttage, float _slowDuration)
    {

    }
    protected virtual void ReturnDefaultSpeed()
    {
        anim.speed=1.0f;
    }
    public virtual void SetupKnockbackDir(Transform _damageDir)
    {
        if(_damageDir.position.x > transform.position.x)
            knockbackDir=-1;
        else if(_damageDir.position.x < transform.position.x)
            knockbackDir =1;
        
    }
    protected virtual void SetupZeroKnockbackPower()
    {

    }
        
    public virtual void DamageImpact() => StartCoroutine("HitKnockback");
    public virtual void Die()
    {

    }
    public void SetupKnockbackPower(Vector2 knockbackpower) => knockbackPower = knockbackpower;
    protected virtual IEnumerator HitKnockback()
    {

        isKnocked=true;
        myrb.velocity = new Vector2(knockbackPower.x * knockbackDir,knockbackPower.y);
        yield return new WaitForSeconds(knockbackDuration);
        isKnocked = false;
        SetupZeroKnockbackPower();
    }

    #region Velocity      
    public void ZeroVelocity()
    {
        if(isKnocked)
            return;
        myrb.velocity=new Vector2(0,0);
    }
    public void SetVelocity(float _xVelocity,float _yVelocity)
    {
        if(isKnocked)
            return;
        myrb.velocity = new Vector2(_xVelocity,_yVelocity);
        FlipController(_xVelocity);
        
        
    }
    #endregion

    #region Collision
    public virtual bool IsGroundDetected() => Physics2D.Raycast(groundCheck.position, Vector2.down,groundCheckDistance,whatIsGround); 
    public virtual bool IsWallDetected() => Physics2D.Raycast(wallCheck.position,Vector2.right * facingDir,wallCheckDistance,whatIsGround); 
    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position,new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        Gizmos.DrawLine(wallCheck.position,new Vector3(wallCheck.position.x + wallCheckDistance * facingDir,wallCheck.position.y));
        Gizmos.DrawWireSphere(attackCheck.position, attackCheckRadius);

    }
    #endregion
    #region Flip
    public virtual void Flip()
    {
        facingDir*=-1;
        facingRight=!facingRight;
        transform.Rotate(0,180,0);
        if(onFlipped != null)
            onFlipped();
    }
    public virtual void FlipController(float _x)
    {
        if(_x > 0 && !facingRight)
            Flip();
        else if(_x < 0 && facingRight)
            Flip();
    }
    #endregion

    
}
