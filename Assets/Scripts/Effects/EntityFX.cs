using System.Collections;
using Cinemachine;
using TMPro;
using UnityEngine;

public class EntityFX : MonoBehaviour
{
    protected Player player;
    protected SpriteRenderer sr; 

    [Header("Pup Up Text")]
    [SerializeField] private GameObject pupUpTextPrefab;




    [Header("Flash FX")]
    [SerializeField] private float flashDuration;
    [SerializeField] private Material hitMat;
    private Material originalMat;

    [Header("Ailment colors")]
    [SerializeField] private Color[] chillColor;
    [SerializeField] private Color[] igniteColor;
    [SerializeField] private Color[] shockColor;
    [Header("Ailment particles")]
    [SerializeField] private ParticleSystem igniteFx;
    [SerializeField] private ParticleSystem chillFx;
    [SerializeField] private ParticleSystem shockFx;

    [Header("Hit FX")]
    [SerializeField] private GameObject hitFx;
    [SerializeField] private GameObject criticalHitFx;




    protected virtual void Start() {
        player = PlayerManager.instance.player;
        sr = GetComponentInChildren<SpriteRenderer>();
        originalMat = sr.sharedMaterial;
    }

    public void CreatePupUpText(string _text)
    {
        float randomX = Random.Range(-1, 1);
        float randomY = Random.Range(1.5f, 3);

        Vector3 positionOffset = new Vector3(randomX,randomY,0);
        GameObject newText = Instantiate(pupUpTextPrefab,transform.position + positionOffset,Quaternion.identity);
        newText.GetComponent<TextMeshPro>().text = _text;
    }


    public void MakeTransprent(bool _tranprent)
    {
        if(_tranprent)
            sr.color = Color.clear;
        else
            sr.color = Color.white;
    }
    public IEnumerator FlashFX()
    {
        sr.material=hitMat;
        Color currentColor = sr.color;
        sr.color = Color.white;
        yield return new WaitForSeconds(flashDuration);
        sr.color =currentColor;
        sr.material=originalMat;

        
    }
    private void RedColorBlink()
    {
        if(sr.color!=Color.white)
        {
            sr.color=Color.white;
        }
        else{
            sr.color=Color.red;
        }
    }
    private void CancelColorChange()
    {
        CancelInvoke();
        sr.color=Color.white;
        igniteFx.Stop();
        chillFx.Stop();
        shockFx.Stop();

    }
    public void IgniteFxFor(float _second)
    {
        igniteFx.Play();
        InvokeRepeating("IgniteColorFx", 0, .3f);
        Invoke("CancelColorChange",_second);
    }
    public void ChillFxFor(float _second)
    {
        chillFx.Play();
        InvokeRepeating("ChillColorFx", 0, .3f);
        Invoke("CancelColorChange",_second);
    }
    public void ShockFxFor(float _second)
    {
        shockFx.Play();
        InvokeRepeating("ShockColorFx", 0, .3f);
        Invoke("CancelColorChange",_second);
    }
    private void IgniteColorFx()
    {
        if(sr.color != igniteColor[0])
            sr.color=igniteColor[0];
        else
            sr.color=igniteColor[1];
    }
    private void ChillColorFx()
    {
        if(sr.color!= chillColor[0])
            sr.color=chillColor[0];
        else
            sr.color = chillColor[1];
        
    }
    private void ShockColorFx()
    {
        if(sr.color != shockColor[0])
            sr.color = shockColor[0];
        else
            sr.color = shockColor[1];
    }
    public void CreateHitFX(Transform _target,bool _critical)
    {
        float zRotation = Random.Range(-90f, 90f);
        float xPosition = Random.Range(-.5f, .5f);
        float yPosition = Random.Range(-.5f, .5f);
        Vector3 hitFxRotation = new Vector3(0,0,zRotation);  
        GameObject hitPrifab= hitFx;
        if(_critical)
        {
            hitPrifab = criticalHitFx;
            float yRotation =0;
            zRotation =Random.Range(-45,45);
            if(GetComponent<Entity>().facingDir == -1)
                yRotation =180;
            hitFxRotation = new Vector3(0,yRotation,zRotation);
        }

        GameObject newHitFx =Instantiate(hitPrifab, _target.position + new Vector3(xPosition,yPosition), Quaternion.identity);

        newHitFx.transform.Rotate(hitFxRotation);
        Destroy(newHitFx, .5f);
    }

}
