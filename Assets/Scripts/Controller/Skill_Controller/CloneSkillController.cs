using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CloneSkillController : MonoBehaviour
{
    private Player player;
    private SpriteRenderer sr;
    private  Animator anim;
    private float cloneTimer;
    private float attackMultiplier;
    [SerializeField] private float colorLoosingSpeed;
    [SerializeField] private Transform attackCheck;
    [SerializeField] private float attackCheckRadius=.8f;
    private int facingDir=1;
    private bool canDuplicateClone;
    private float chanceToDuplicate;
    [Space]
    [SerializeField] private LayerMask whatIsEnemy;
    [SerializeField] private float closestEnemyCheckRadius=25;
    [SerializeField] private Transform closestEnemy;



    private void Awake() {
        sr = GetComponent<SpriteRenderer>();
        anim= GetComponent<Animator>();
        StartCoroutine(FaceClosestTaget());
    }
    
    private void Update() {
        cloneTimer -= Time.deltaTime;
        if(cloneTimer<0)
        {
            sr.color = new Color(1,1,1,sr.color.a - (Time.deltaTime * colorLoosingSpeed));
            if(sr.color.a < 0)
                Destroy(gameObject);
        }
    }
    // setup position, bonus position, can active to attack, direction
    public void SetUpClone(Transform _newPosition,float _cloneDuration,bool _canAttack,Vector3 _offset,
       bool _canDuplicateClone,float _chanceToDuplicate,Player _player, float _attackMultiplier)
    {
        if(_canAttack)
            anim.SetInteger("AttackNumber", Random.Range(1,4));

        attackMultiplier = _attackMultiplier;
        player =_player;
        transform.position = _newPosition.position + _offset;       
        cloneTimer=_cloneDuration;
    
        canDuplicateClone=_canDuplicateClone;
        chanceToDuplicate=_chanceToDuplicate;
    }

    private void AnimationTrigger()
    {
        cloneTimer=-1f;
    }
    // take damage
    private void AttackTriggers()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(attackCheck.position,attackCheckRadius);
        foreach(var hit in colliders)
        {
            if(hit.GetComponent<Enemy>() != null)
            {
                //player.stats.DoDamage(hit.GetComponent<CharacterStats>());
                hit.GetComponent<Entity>().SetupKnockbackDir(transform);
                PlayerStats playerStats = player.GetComponent<PlayerStats>();
                EnemyStats enemyStats = hit.GetComponent<EnemyStats>();


                playerStats.CloneDoDamage(enemyStats,attackMultiplier);

                Inventory.instance.GetEquipment(EquipmentType.Weapon)?.Effect(hit.transform);


                if(canDuplicateClone)
                {
                    if(Random.Range(0,100) < chanceToDuplicate)
                    {
                        SkillManager.instance.clone.CreateClone(hit.transform,new Vector3(0.5f * facingDir,0));
                    }
                }
            }
        }
    }

    //detective enemy direction
    private IEnumerator FaceClosestTaget()
    {
        yield return null;
        FindClosestEnnemy();
        if(closestEnemy!=null)
        {
            if(transform.position.x > closestEnemy.position.x)
            {
                facingDir=-1;
                transform.Rotate(0,180,0);
            }
        }
    }
   private void FindClosestEnnemy()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position,closestEnemyCheckRadius,whatIsEnemy); 

        float closestDistance =Mathf.Infinity;

        foreach(var hit in colliders)
        {

            float distanceToEnemy=Vector2.Distance(transform.position,hit.transform.position);
            if(distanceToEnemy < closestDistance)
            {
                closestDistance = distanceToEnemy;
                closestEnemy = hit.transform;
            }
            
        }
    }
    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position,closestEnemyCheckRadius);
    }
}
