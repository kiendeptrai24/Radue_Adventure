using System.Collections;
using UnityEngine;

public class Player : Entity
{
    [Header("Attack details")]
    public Vector2[] attackMovement;
    public float counterAtackDuration=.2f;
    public bool isBusy { get; private set; }

    [Header("Move info")]
    public float moveSpeed = 12;
    public float jumpForce;
    public float swordReturnImpact;
    private float defaultMoveSpeed;
    private float defaultJumpForce;

    [Header("Dash info")]
    public float dashSpeed;
    public float dashDuration;
    public float defaultDashSpeed;
    public float dashDir { get; private set; }
   
    [Header("Skill Manager")]
    public bool hasSword=false;
    public SkillManager skill{get; private set;}
    public GameObject sword {get; private set;}
    public PlayerFX fx {get; private set;}
   
    #region States
    public PlayerStateMachine stateMachine {get; private set;}
    public PlayerIdleState idleState {get; private set;}
    public PlayerMoveState moveState {get; private set;}
    public PlayerJumpState jumpState {get; private set;}
    public PlayerAirState airState {get; private set;}
    public PlayerWallSlideState wallSlideState {get; private set;}
    public PlayerWallJumpState WallJumpState {get; private set;}
    public PlayerPrimaryAttackState primaryAttackState {get; private set;}
    public PlayerDashState dashState {get; private set;}
    public PlayerCounterAttackState counterAttackState{get; private set;}
    public PlayerAimSwordState aimSwordState{get; private set;}
    public PlayerCatchSwordState catchSwordState{get; private set;}
    public PlayerBlackholeState blackholeState{get; private set;}
    public PlayerDeadState deadState{get; private set;}
    #endregion
   


    protected override void Awake() {
        base.Awake();
        nameOfChar = "Radue";
        //state machine
        stateMachine = new PlayerStateMachine();
        //state player
        idleState = new PlayerIdleState(this,stateMachine,"Idle");
        moveState = new PlayerMoveState(this,stateMachine,"Move");
        jumpState = new PlayerJumpState(this,stateMachine,"Jump");
        airState  = new PlayerAirState(this,stateMachine,"Jump");
        dashState = new PlayerDashState(this,stateMachine, "Dash");
        wallSlideState = new PlayerWallSlideState(this,stateMachine,"WallSlide");
        WallJumpState = new PlayerWallJumpState(this,stateMachine,"Jump");
        //state player attack
        primaryAttackState = new PlayerPrimaryAttackState(this,stateMachine,"Attack");
        counterAttackState = new PlayerCounterAttackState(this,stateMachine,"CounterAttack");
        //state Player uses Skill 
        aimSwordState = new PlayerAimSwordState(this,stateMachine,"AimSword");
        catchSwordState = new PlayerCatchSwordState(this,stateMachine,"CatchSword");
        blackholeState = new PlayerBlackholeState(this,stateMachine,"Jump");

        deadState = new PlayerDeadState(this,stateMachine,"Die");


    }

    protected override void Start() 
    {
        base.Start();
        fx = GetComponent<PlayerFX>();
        skill = SkillManager.instance;
        stateMachine.Initialize(idleState);   
        defaultMoveSpeed =moveSpeed;
        defaultJumpForce = jumpForce; 
        defaultDashSpeed = dashSpeed;
    }
    
    protected override void Update() 
    {
        if(Time.timeScale == 0)
            return;
        base.Update();
        stateMachine.currentState.Update();
        CheckForDashInput();
        if(Input.GetKeyDown(KeyCode.F) && skill.crystal.crystalUnlocked)
            skill.crystal.CanUseSkill();

        if(Input.GetKeyDown(KeyCode.Alpha1))
            Inventory.instance.UseFlask();
        
    }
    public override void SlowEntityBy(float _slowPercenttage, float _slowDuration)
    {
        moveSpeed = moveSpeed * (1 - _slowPercenttage);
        jumpForce = jumpForce * (1 - _slowPercenttage);
        dashSpeed = dashSpeed * (1 - _slowPercenttage);
        anim.speed = anim.speed * (1 - _slowPercenttage);

        Invoke("ReturnDefaultSpeed",_slowDuration);
    }
    protected override void ReturnDefaultSpeed()
    {
        base.ReturnDefaultSpeed();

        moveSpeed = defaultMoveSpeed;
        jumpForce = defaultJumpForce;
        dashSpeed = defaultDashSpeed;
    }
    public void AssignNewSword(GameObject _newSword)
    {
        sword = _newSword;
    }
    public void CatchTheSword()
    {
        stateMachine.ChangeState(catchSwordState);
        Destroy(sword);
    }
    public IEnumerator BusyFor(float _second)
    {
        isBusy =true;
        yield return new WaitForSeconds(_second);
        isBusy =false;
    }
    public void AnimationTrigger() => stateMachine.currentState.AnimationFinishTrigger();
    private void CheckForDashInput() 
    {
        if(IsWallDetected())
            return;
        if(skill.dash.dashUnlocked ==false)
            return;
        if (Input.GetKeyDown(KeyCode.LeftShift) && SkillManager.instance.dash.CanUseSkill())
        {
            dashDir = Input.GetAxisRaw("Horizontal");
                if(dashDir==0)
                    dashDir=facingDir;
            stateMachine.ChangeState(dashState);
        }
    }
    protected override void SetupZeroKnockbackPower()
    {
        knockbackPower = new Vector2(0,0);
    }
    public override void Die()
    {
        base.Die();
        stateMachine.ChangeState(deadState);
    }

}
