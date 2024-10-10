using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    private Animator anim;
    public string id;
    public bool activationStatus;

    private void Awake() {
        anim = GetComponent<Animator>();
    }
    [ContextMenu("Generate checkpoint Id")]
    private void GenerateID()
    {
        id = System.Guid.NewGuid().ToString();
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.GetComponent<Player>() != null)
        {
            ActivateCheckPoint();
        }
    }

    public void ActivateCheckPoint()
    {
        if(!activationStatus)
            AudioManger.instance.PlayerSFX(5,transform);
        activationStatus = true;
        anim.SetBool("Active", true);
    }
}
