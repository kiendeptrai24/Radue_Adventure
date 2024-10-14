
using Cinemachine;
using UnityEngine;

public class PlayerFX : EntityFX
{
    
    [Header("Screen shake FX")]
    [SerializeField] private float shakeMultiplier;
    private CinemachineImpulseSource screenShake;
    public Vector3 swordImpactShake;
    public Vector3 shakeHighDamage;
    [Header("After image fx")]
    [SerializeField] private GameObject afterImagePrefab;
    [SerializeField] private float colorLooseRate;
    [SerializeField] private float afterImageCooldown;
    private float afterImageCooldownTimer;
    [Space]
    [SerializeField] private ParticleSystem dustFx;
    protected override void Start()
    {
        base.Start();
        screenShake = GetComponent<CinemachineImpulseSource>();

    }
    protected void Update()
    {
        afterImageCooldownTimer -= Time.deltaTime;

    }
        public void CreateAfterImage()
    {
        if(afterImageCooldownTimer < 0)
        {
            afterImageCooldownTimer = afterImageCooldown;
            GameObject newAfterImage = Instantiate(afterImagePrefab, transform.position,transform.rotation);
            newAfterImage.GetComponent<AfterImageFX>().SetupAfterImage(colorLooseRate,sr.sprite);
        }
    }
    public void ScreenShake(Vector3 _shakePower)
    {
        screenShake.m_DefaultVelocity = new Vector3(_shakePower.x * player.facingDir,_shakePower.y) * shakeMultiplier;
        screenShake.GenerateImpulse();
    }
    public void PlayDustFX()
    {
        if(dustFx !=  null)
            dustFx.Play();
    }


}
