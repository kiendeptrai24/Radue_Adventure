using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Crystal_Skill_controller : MonoBehaviour
{
    private Player player;
    private Animator anim => GetComponent<Animator>();
    private CircleCollider2D cd => GetComponent<CircleCollider2D>();
    private float crystalExistTimer;
    private bool canExplode;
    private bool canMove;
    private float moveSpeed;

    private bool canGrow;
    private float growSpeed =5;
    private Transform closestTarget;

    [SerializeField] private LayerMask whatIsEnemy;
    public void SetupCrystal(float crystalDuration, bool _canExplode, bool _canMove,float _moveSpeed,Transform _closestTarget,Player _player)
    {
        player=_player;
        crystalExistTimer = crystalDuration;
        canExplode = _canExplode;
        canMove = _canMove;
        moveSpeed = _moveSpeed;
        closestTarget = _closestTarget;
    }
    private void Update()
    {
        crystalExistTimer-=Time.deltaTime;
        if (crystalExistTimer < 0)
        {
            FinishCrystal();
        }   
        if(canMove)
        {
            if(closestTarget ==null)
                return;
            transform.position = Vector2.MoveTowards(transform.position, closestTarget.position,moveSpeed * Time.deltaTime);
            if(Vector2.Distance(transform.position, closestTarget.position)<1)
            {
                FinishCrystal();
                canMove=false;  
            }
        }
        if(canGrow)
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(3,3), growSpeed * Time.deltaTime);
    }
    //new
    public void ChooseRandomEnemy()
    {
        float radius = SkillManager.instance.blackhole.GetBlackholeRadios();
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position,radius,whatIsEnemy);
        if(colliders.Length>0)
            closestTarget = colliders[Random.Range(0,colliders.Length)].transform;
    }
    public void FinishCrystal()
    {
        if (canExplode)
        {
            canGrow=true;
            anim.SetTrigger("Explode");
        }
        else
        {
            SelfDestroy();
        }
    }
    private void AnimationExplodeEvent()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position,cd.radius);
        foreach(var hit in colliders)
        {
            if(hit.GetComponent<Enemy>() != null)
            {
                player.stats.DoMagicalDamage(hit.GetComponent<CharacterStats>());

                ItemData_Equipment equipedAmulet = Inventory.instance.GetEquipment(EquipmentType.Amulet);
                if(equipedAmulet != null)
                    equipedAmulet.Effect(hit.transform);
            }
        }
    }
    public void SelfDestroy() => Destroy(gameObject);
}
