using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosive_Controller : MonoBehaviour
{
    private Animator anim;
    private CharacterStats stats;
    private float growSpeed = 15;
    private float maxSize= 6;
    private float ExplorsionRadius;

    private bool canGrow = true;
    private void Update() {
        if(canGrow)
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(maxSize, maxSize), growSpeed * Time.deltaTime);
        if(maxSize - transform.localScale.x < .5f)
        {
            canGrow = false;
            anim.SetTrigger("Explode");
        }

    }

    public void SetupExplosive(CharacterStats _stats, float _growSpeed, float _maxSize, float _explorsionRadius)
    {
        anim = GetComponent<Animator>();
        this.stats = _stats;
        this.growSpeed = _growSpeed;
        this.maxSize = _maxSize;
        this.ExplorsionRadius = _explorsionRadius;
    }
    private void AnimationExplodeEvent()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position,ExplorsionRadius);
        foreach(var hit in colliders)
        {
            if(hit.GetComponent<CharacterStats>() != null)
            {
                hit.GetComponent<Entity>().SetupKnockbackDir(transform);
                stats.DoDamage(hit.GetComponent<CharacterStats>());
            }
        }
    }
    private void SeftDestroy() => Destroy(gameObject);
    
}
