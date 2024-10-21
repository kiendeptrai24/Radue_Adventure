using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow_Controller : MonoBehaviour
{
    [SerializeField] private string targetLayerName = "Player";
    [SerializeField] private float xVelocity;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private bool canMove;
    [SerializeField] private bool flipped;
    private CharacterStats stats;
    private void Update() {
        if(canMove)
            rb.velocity = new Vector2(xVelocity,rb.velocity.y);
    }
    public void SetupArrow(float _speed,CharacterStats _stats)
    {
        xVelocity = _speed;
        stats = _stats;
        if(_speed <0)
            transform.Rotate(0,180,0);
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.layer == LayerMask.NameToLayer(targetLayerName))
        {
            //other.GetComponent<CharacterStats>()?.TakeDamage(damage);
            stats.DoDamage(other.GetComponent<CharacterStats>());
            StuckInto(other);
        }
        else if(other.gameObject.layer == LayerMask.NameToLayer("Ground"))
            StuckInto(other);
    }

    private void StuckInto(Collider2D other)
    {
        GetComponentInChildren<ParticleSystem>().Stop();
        GetComponent<CapsuleCollider2D>().enabled = false;
        canMove = false;
        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        transform.parent = other.transform;
        Destroy(gameObject,Random.Range(5,7));
    }

    public void FlipArrow()
    {
        if(flipped)
            return;
        xVelocity = xVelocity * -1;
        flipped= true;
        transform.Rotate(0,180,0);
        targetLayerName = "Enemy";
    }
}
