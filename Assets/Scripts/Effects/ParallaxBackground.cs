using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    GameObject cam;
    [SerializeField] float parallaxEffect;
    private float xPos;
    private float length;
    void Start()
    {
        cam =GameObject.Find("Main Camera");
        length =GetComponent<SpriteRenderer>().bounds.size.x;
        xPos = cam.transform.position.x;
       
    }

    // Update is called once per frame
    void Update()
    {
        
        float distanceMoved=cam.transform.position.x * (1-parallaxEffect);
        float distanceToMove = cam.transform.position.x * parallaxEffect;
        
        transform.position = new Vector3(xPos + distanceToMove,transform.position.y);
        if(distanceMoved > xPos + length)
        {
            xPos += length;
        }    
        else if(distanceMoved < xPos - length)
        {
            xPos-=length;
        }
    }

    
}
