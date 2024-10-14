using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEditor;
using UnityEngine;

public class Sword_Skill_Controller : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;
    private CircleCollider2D cd;
    private Player player;
    private bool CanRotate=true;
    private bool isReturning;


    private float freezeTimeDuration;
    private float returnSpeed = 12; 

    [Header("Pierce info")]
    private float pierceAmount;



    [Header("Bounce info")]
    private float bounceSpeed;
    private bool isBouncing;
    private int bounceAmount;
    private List<Transform> enemyTarget;
    private int targetIndex;
    [Header("Spin info")]
    private float maxTravelDistance;
    private float spinDuration;
    private float spinTimer;
    private bool wasStopped;
    private bool isSpinning;
    private float hitTimer;
    private float hitCooldown;
    private float spinDirection;


    //get component
    private void Awake() {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        cd = GetComponent<CircleCollider2D>();
    }
    private void DestroySword() 
    {
        Destroy(gameObject);
    }

    //setupping property the same for sword
    public void SetupSword(Vector2 _dir, float _gravityScale,Player _player,float _freezeTimeDuration,float _returnSpeed)
    {
        player = _player;
        rb.velocity=_dir;
        rb.gravityScale=_gravityScale;
        freezeTimeDuration=_freezeTimeDuration;
        returnSpeed =_returnSpeed;
        if(pierceAmount <= 0)
            anim.SetBool("Rotation",true);
        spinDirection =Mathf.Clamp(rb.velocity.x,-1,1);

        Invoke("DestroySword",7);
    }
    //setup property for the sword Bounce
    public void SetupBounce(bool _isBouncing,int _amountOfBounce,float _bounceSpeed)
    {
        isBouncing=_isBouncing;
        bounceAmount=_amountOfBounce;
        bounceSpeed=_bounceSpeed;
        
        enemyTarget = new List<Transform>();
    }
    //setup property for the sword Pierce
    public void SetupPierce(int _pierceAmount)
    {
        pierceAmount=_pierceAmount;
    }
    //setup property for the sword spin
    public void SetupSpin(bool _isSpinning,float _maxTravelDistance,float _spinDuration,float _hitCooldown)
    {
        isSpinning=_isSpinning;
        maxTravelDistance=_maxTravelDistance;
        spinDuration=_spinDuration;
        hitCooldown=_hitCooldown;
    }
    // movement of the sword
    private void Update()
    {
        if (CanRotate)
            transform.right = rb.velocity;
        if (isReturning)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, returnSpeed * Time.deltaTime);
            if (Vector2.Distance(transform.position, player.transform.position) < 1)
            {
                anim.SetBool("Rotation", false);
                player.CatchTheSword();
            }

        }
        BounceLogic();
        SpinLogic();
    }
    //Logic of the sword Spin
    private void SpinLogic()
    {
        if (isSpinning)
        {
            if (Vector2.Distance(player.transform.position, transform.position) > maxTravelDistance && !wasStopped)
            {
                StopWhenSpinning();
            }
            if (wasStopped)
            {
                spinTimer -= Time.deltaTime;
                transform.position= Vector2.MoveTowards(transform.position,new Vector2(transform.position.x +spinDirection,transform.position.y), 1.5f * Time.deltaTime);
                if (spinTimer <= 0)
                {
                    isReturning = true;
                    isSpinning = false;
                }
                hitTimer -= Time.deltaTime;
                if (hitTimer <= 0)
                {
                    hitTimer = hitCooldown;

                    Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 1);
                    foreach (var hit in colliders)
                    {
                        if (hit.GetComponent<Enemy>() != null)
                            SwordSkillDamage(hit.GetComponent<Enemy>());
                    }
                }
            }
        }
    }
    // state of the sword will stop position
    private void StopWhenSpinning()
    {
        wasStopped = true;
        rb.constraints = RigidbodyConstraints2D.FreezePosition;
        spinTimer = spinDuration;
    }
    //logic of the sword Bounce 
    private void BounceLogic()
    {
        if(Input.GetKeyDown(KeyCode.Mouse1))
        {
            isBouncing=false;
            isReturning=true;
        }
        
        if (isBouncing && enemyTarget.Count > 0)
        {
            transform.position = Vector2.MoveTowards(transform.position, enemyTarget[targetIndex].position, bounceSpeed * Time.deltaTime);
            if (Vector2.Distance(transform.position, enemyTarget[targetIndex].position) < .1f)
            {
                SwordSkillDamage(enemyTarget[targetIndex].GetComponent<Enemy>());

                targetIndex++;
                bounceAmount--;
                if (bounceAmount <= 0)
                {
                    isBouncing = false;
                    isReturning = true;
                }
                if (targetIndex >= enemyTarget.Count)
                    targetIndex = 0;
            }
        }
    }
    // state of the sword change to come to player
    public void ReturnSword()
    {
        rb.constraints =RigidbodyConstraints2D.FreezeAll;
        transform.parent = null;
        isReturning = true;

        //set cooldown
    }
    //trigger system
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(isReturning)
            return;
        
        if(collision.GetComponent<Enemy>() != null)
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            SwordSkillDamage(enemy);
        }

        //collision.GetComponent<Enemy>()?.Damage();

        SetupTargetsForBounce(collision);

        StuckInto(collision);
    }

    private void SwordSkillDamage(Enemy enemy)
    {
        EnemyStats enemyStats = enemy.GetComponent<EnemyStats>();
        player.stats.DoDamage(enemyStats);
        
        if(player.skill.sword.timeStopUnlocked)
            enemy.FreezeTimefor(freezeTimeDuration);
        if(player.skill.sword.vulnerableUnlocked)
            enemyStats.MakeVulnerablefor(freezeTimeDuration);
        ItemData_Equipment equipedAmulet = Inventory.instance.GetEquipment(EquipmentType.Amulet);
            if(equipedAmulet != null)
                equipedAmulet.Effect(enemy.transform);

    }

    // check trigger bounce skill
    private void SetupTargetsForBounce(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)
        {
            if (isBouncing && enemyTarget.Count <= 0)
            {
                anim.SetBool("Rotation", true);

                Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 10);
                foreach (var hit in colliders)
                {
                    if (hit.GetComponent<Enemy>() != null)
                        enemyTarget.Add(hit.transform);
                }
            }
        }
    }
    // translate components for the sword
    private void StuckInto(Collider2D collision)
    {
        
      
        if(pierceAmount > 0 && collision.GetComponent<Enemy>() != null)
        {
            pierceAmount--;
            return;
        }
        if(isSpinning)
        {
            return;
        }
        CanRotate = false;
        cd.enabled = false;
        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        GetComponentInChildren<ParticleSystem>().Play();
        if(isBouncing && enemyTarget.Count>0)
            return;
        anim.SetBool("Rotation", false);
        transform.parent = collision.transform;
    }
}
