using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PupUpTextFX : MonoBehaviour
{
    private TextMeshPro myTest;
    [SerializeField] private float speed;
    [SerializeField] private float desapiearanceSpeed;
    [SerializeField] private float colorDesapiearanceSpeed;
    [SerializeField] private float lifeTime;
    private float textTimer;
    void Start()
    {
        myTest = GetComponent<TextMeshPro>();
        textTimer = lifeTime;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position,new Vector2(transform.position.x, transform.position.y +1),speed * Time.deltaTime);
        textTimer -= Time.deltaTime;
        if(textTimer < 0)
        {
            float alpha = myTest.color.a - colorDesapiearanceSpeed * Time.deltaTime;
            myTest.color = new Color(myTest.color.r, myTest.color.g, myTest.color.b, alpha);

            if(myTest.color.a < 50)
                speed = desapiearanceSpeed;    
            if(myTest.color.a <= 0)
                Destroy(gameObject);
        }

    }
}
