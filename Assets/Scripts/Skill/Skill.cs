using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    public float cooldown;
    public float cooldownTimer;
    protected Player player;
    protected bool canClickSkill;
    protected virtual void Start() {
        player=PlayerManager.instance.player;
        Invoke(nameof(CheckUnlock),1);
        Invoke(nameof(AddButtonSkillTree),.1f);
    }
    protected virtual void Update()
    {
        cooldownTimer -= Time.deltaTime;
    }
    public virtual bool CanUseSkill()
    {
        if(cooldownTimer < 0)
        {
            UseSkill();
            cooldownTimer = cooldown;
            return true;
        }
        else if(canClickSkill)
        {
            UseSkill();
            return false;
        }
        player.fx.CreatePupUpText("Cooldown");
        return false;
    }
    protected virtual void AddButtonSkillTree()
    {

    }
    protected virtual void CheckUnlock()
    {

    }
    public virtual void UseSkill()
    {
        
        //do some skill specific things
    }
    protected virtual Transform FindClosestEnnemy(Transform _checkTranform)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(_checkTranform.position,25); 

        float closestDistance =Mathf.Infinity;
        Transform closestEnemy =null;

        foreach(var hit in colliders)
        {
            if(hit.GetComponent<Enemy>()!=null)
            {
                float distanceToEnemy=Vector2.Distance(_checkTranform.position,hit.transform.position);
                if(distanceToEnemy < closestDistance)
                {
                    closestDistance = distanceToEnemy;
                    closestEnemy = hit.transform;
                }
            }
        }
        return closestEnemy;
    }
}
