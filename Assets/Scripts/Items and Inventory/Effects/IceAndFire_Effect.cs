using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Ice And Fire efftect", menuName = "Data/Item effect/Ice And Fire")]
public class IceAndFire_Effect : Item_Effect
{
    [SerializeField] private GameObject iceAndFirePrefab;
    [SerializeField] private Vector2 newVelocity;
    
    public override void ExecuteEffect(Transform _respawnPosition)
    {
        Player player = PlayerManager.instance.player;

        bool thirdAttack = player.primaryAttackState.comboCounter == 2;

        if(thirdAttack)
        {
            AudioManger.instance.PlayerSFX(17,null);
            GameObject newIceAndFire = Instantiate(iceAndFirePrefab,_respawnPosition.position, player.transform.rotation);
            newIceAndFire.GetComponent<Rigidbody2D>().velocity = new Vector2(newVelocity.x * player.facingDir, newVelocity.y);
            Destroy(newIceAndFire,2);
        }
    }
}
