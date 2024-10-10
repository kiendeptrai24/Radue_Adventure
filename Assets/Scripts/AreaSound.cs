using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaSound : MonoBehaviour
{
    [SerializeField] private int areaSoundIndex;

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.GetComponent<Player>())
        {
            AudioManger.instance.PlayerSFX(areaSoundIndex,null);
        }
    }
    private void OnTriggerExit2D(Collider2D other) {
         if(other.GetComponent<Player>())
        {
            AudioManger.instance.StopSFXWithTime(areaSoundIndex);
        }
    }
}
