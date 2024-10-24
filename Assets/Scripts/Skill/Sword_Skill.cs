using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
public enum SwordType{
    Regular,
    Bounce,
    Pierce,
    Spin
}
public class Sword_Skill : Skill
{

    public SwordType swordType=SwordType.Regular;


    [Header("Bounce info")]
    [SerializeField] private UI_SkillTreeSlot bounceUnlockButton;

    [SerializeField] private int bounceAmount;
    [SerializeField] private float bounceGravity;
    [SerializeField] private float bounceSpeed;

    [Header("Peirce info")]
    [SerializeField] private UI_SkillTreeSlot pierceUnlockButton;
    [SerializeField] private int pierceAmount;
    [SerializeField] private float pierceGravity;
    [Header("Spin info")]
    [SerializeField] private UI_SkillTreeSlot spinUnlockButton;
    [SerializeField] private float hitCooldown=.35f;
    [SerializeField] private float maxTravelDistance=7;
    [SerializeField] private float spinDuration=2;
    [SerializeField] private float spinGravity=1;

    [Header("Skill info")]
    [SerializeField] private UI_SkillTreeSlot swordUnlockButton;
    public bool swordUnlocked {get; private set;} 

    [SerializeField] private GameObject swordPrefab;
    [SerializeField] private Vector2 launchForce;
    [SerializeField] private float swordGravity;
    [SerializeField] private float freezeTimeDuration;
    [SerializeField] private float returnSpeed;


    [Header("Passive skill")]
    [SerializeField] private UI_SkillTreeSlot timeStopUnlockButton;
    public bool timeStopUnlocked {get; private set;}
    [SerializeField] private UI_SkillTreeSlot vulnerableUnlockButton;
    public bool vulnerableUnlocked {get; private set;}


    private Vector2 finalDir;

    [Header("Aim dots")]
    [SerializeField] private float spaceBeetwenDots;

    protected override void Start()
    {
        base.Start();
        SetupGravity();

       
    }
    protected override void AddButtonSkillTree()
    {
        timeStopUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockTimeStop);
        vulnerableUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockVulnerable);
        swordUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockSword);
        bounceUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockBounceSword);
        pierceUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockPierceSword);
        spinUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockSpinSword);
    }  



    #region Unlock region

    protected override void CheckUnlock()
    {
        UnlockSword();
        UnlockBounceSword();
        UnlockSpinSword();
        UnlockPierceSword();
        UnlockTimeStop();
        UnlockVulnerable();
    }
    private void UnlockTimeStop()
    {
        if(timeStopUnlockButton.unlocked)
            timeStopUnlocked = true;
    }
    private void UnlockVulnerable()
    {
        if(vulnerableUnlockButton.unlocked)
            vulnerableUnlocked = true;
    }
    private void UnlockSword()
    {
        if(swordUnlockButton.unlocked)
        {
            swordType = SwordType.Regular;
            swordUnlocked = true;
        }
    }

    private void UnlockBounceSword()
    {
        if(bounceUnlockButton.unlocked)
            swordType = SwordType.Bounce;
    }
    private void UnlockPierceSword()
    {
        if(pierceUnlockButton.unlocked)
            swordType = SwordType.Pierce;
    }
    private void UnlockSpinSword()
    {
        if(spinUnlockButton.unlocked)
            swordType = SwordType.Spin;
    }

    #endregion

    private void SetupGravity()
    {
        if(swordType == SwordType.Bounce)
            swordGravity = bounceAmount;
        else if(swordType == SwordType.Pierce)
            swordGravity = pierceGravity;
        else if(swordType == SwordType.Spin)
            swordGravity = spinGravity;
    }

    protected override void Update() {
        //add here
        SetupGravity();

        if(Input.GetKeyUp(KeyCode.Mouse1)) 
        {
            finalDir = new Vector2(AimDirection().normalized.x * launchForce.x, AimDirection().normalized.y * launchForce.y);
        }
        if(Input.GetKey(KeyCode.Mouse1))
        {
            for (int i = 0; i < ObjectPooling.instance.amountOfPool; i++)
            {
                ObjectPooling.instance.GetObject(i).transform.position =  DotsPosision(i * spaceBeetwenDots);
            }
        }
    }
    public void CreateSword()
    {
        GameObject newSword = Instantiate(swordPrefab,player.transform.position,transform.rotation);
        Sword_Skill_Controller newSwordScript = newSword.GetComponent<Sword_Skill_Controller>();

        if(swordType==SwordType.Bounce)
            newSwordScript.SetupBounce(true,bounceAmount,bounceSpeed);
        else if(swordType==SwordType.Pierce)
            newSwordScript.SetupPierce(pierceAmount);
        else if(swordType == SwordType.Spin)
            newSwordScript.SetupSpin(true,maxTravelDistance,spinDuration,hitCooldown);
        
        newSwordScript.SetupSword(finalDir,swordGravity,player,freezeTimeDuration,returnSpeed);
        player.AssignNewSword(newSword);
        ObjectPooling.instance.DotsActive(false);
        
    }
    #region Aim region
    public Vector2 AimDirection()
    {
        Vector2 playerPosision =player.transform.position;
        Vector2 mousePosision = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePosision - playerPosision;

        return direction;
    }

    private Vector2 DotsPosision(float t)
    {
        Vector2 posision = (Vector2)player.transform.position + new Vector2(AimDirection().normalized.x * launchForce.x, 
        AimDirection().normalized.y * launchForce.y) * t + .5f*(Physics2D.gravity * swordGravity) * (t * t);
        return posision;
    }
    #endregion
}
