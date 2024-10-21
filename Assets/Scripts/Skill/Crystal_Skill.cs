using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Crystal_Skill : Skill
{
    [SerializeField] private float crystalDuration;
    [SerializeField] private GameObject crystalPrefab;
    private GameObject currentCrystal;

    [Header("Crystal")]
    [SerializeField] private UI_SkillTreeSlot unlockCrystalButton;
    public bool crystalUnlocked ;//{get; private set;}
    [Header("Crystal mirage")]
    [SerializeField] private UI_SkillTreeSlot unlockCloneInsteadButton;
    [SerializeField] private bool cloneInsteadOfCrystal;
    [SerializeField] private int crystalMirageCooldown;

    [Header("Explosive crystal")]
    [SerializeField] private UI_SkillTreeSlot unlockExplosiveButton;
    [SerializeField] private bool canExplode;
    [SerializeField] private int  crystalExplosiveCooldown;


    [Header("Moving crystal")]
    [SerializeField] private UI_SkillTreeSlot unlockMovingCrystalButton;
    [SerializeField] private bool canMoveToEnemy;
    [SerializeField] private float moveSpeed;
    [SerializeField] private int crystalMoveCooldown;


    [Header("Multi Stacking crystal")]
    [SerializeField] private UI_SkillTreeSlot unlockMultiStackButton;
    [SerializeField] private bool canUseMultiStack;
    [SerializeField] private int amountOfStacks;
    [SerializeField] private float multiStackCooldown;
    [SerializeField] private float useTimeWindow;
    [SerializeField] private int crystalMultiStackCooldown;

    [SerializeField] private List<GameObject> crystalLeft =new List<GameObject>();
    private bool oneTime;

    protected override void Start()
    {
        base.Start();
        
    }
    protected override void AddButtonSkillTree()
    {
        unlockCrystalButton.GetComponent<Button>().onClick.AddListener(UnlockCrystal);
        unlockCloneInsteadButton.GetComponent<Button>().onClick.AddListener(UnlockCrystalMirage);
        unlockExplosiveButton.GetComponent<Button>().onClick.AddListener(UnlockExplosiveCrystal);
        unlockMovingCrystalButton.GetComponent<Button>().onClick.AddListener(UnlockMovingCrystal);
        unlockMultiStackButton.GetComponent<Button>().onClick.AddListener(UnlockMultiStack);
    }
    #region Unlock skill  
    protected override void CheckUnlock()
    {
        UnlockCrystal();
        UnlockCrystalMirage();
        UnlockExplosiveCrystal(); 
        UnlockMovingCrystal();
        UnlockMultiStack();
    }
    private void UnlockCrystal()
    {
        if(unlockCrystalButton.unlocked)
        {
            crystalUnlocked = true;
        }
    }
    private void UnlockCrystalMirage()
    {
        if(unlockCloneInsteadButton.unlocked)
        {
            cloneInsteadOfCrystal =true;
            cooldown = crystalMirageCooldown;
        }   
    }
    private void UnlockExplosiveCrystal()
    {
        if(unlockExplosiveButton.unlocked)
        {
            canExplode = true;
            cooldown = crystalExplosiveCooldown;
        }
    }
    private void UnlockMovingCrystal()
    {
        if(unlockMovingCrystalButton.unlocked)
        {
            canMoveToEnemy=true;
            cooldown = crystalMoveCooldown;
        }
    }
    private void UnlockMultiStack()
    {
        if(unlockMultiStackButton.unlocked)
        {
            canUseMultiStack = true;
            cooldown = crystalMultiStackCooldown;
        }
    }
    #endregion
    public override void UseSkill()
    {
        base.UseSkill();
        if(CanUseMultiCrystal())
            return;

        if(currentCrystal==null)
        {
            CreateCrystal();
        }
        else
        {
            if(canMoveToEnemy)
                return;
            if(oneTime)
            {
                Vector2 playerPos = player.transform.position;
                player.transform.position = currentCrystal.transform.position;
                currentCrystal.transform.position = playerPos;  
                oneTime = false;
            }
            //new
            if(cloneInsteadOfCrystal)
            {
                SkillManager.instance.clone.CreateClone(currentCrystal.transform,Vector3.zero);
                Destroy(currentCrystal);
            }
            else
            {
                currentCrystal.GetComponent<Crystal_Skill_controller>()?.FinishCrystal();
            }

        }
    }

    public void CreateCrystal()
    {
        currentCrystal = Instantiate(crystalPrefab, player.transform.position, Quaternion.identity);
        Crystal_Skill_controller currentCrystalScript = currentCrystal.GetComponent<Crystal_Skill_controller>();
        currentCrystalScript.SetupCrystal(crystalDuration, canExplode, canMoveToEnemy, moveSpeed, FindClosestEnnemy(currentCrystalScript.transform),player);
        oneTime = true;

    }
    //new
    public void CurrentCrystalChooseRandomTarget() => currentCrystal.GetComponent<Crystal_Skill_controller>().ChooseRandomEnemy();

    private bool CanUseMultiCrystal()
    {
        if(canUseMultiStack)
        {
            if(crystalLeft.Count > 0)
            {
                if(crystalLeft.Count == amountOfStacks)
                    Invoke("ResetAbility", useTimeWindow);
                
                cooldown=0;
                GameObject crystalToSpawn=crystalLeft[crystalLeft.Count-1];
                GameObject newCrystal = Instantiate(crystalToSpawn,player.transform.position,Quaternion.identity);
                crystalLeft.Remove(crystalToSpawn);
                newCrystal.GetComponent<Crystal_Skill_controller>().SetupCrystal(crystalDuration,canExplode,canMoveToEnemy,moveSpeed,FindClosestEnnemy(newCrystal.transform),player);
                if(crystalLeft.Count <= 0)
                {
                    cooldown = multiStackCooldown;
                    RefilCrystal();
                }
                return true;
            }

        }

        return false;
    }
    private void RefilCrystal()
    {
        int amountToAdd = amountOfStacks - crystalLeft.Count;
        for (int i = 0; i < amountToAdd; i++)
        {
            crystalLeft.Add(crystalPrefab);
        }   
    }
    private void ResetAbility()
    {
        Debug.Log("reset");
        if(cooldownTimer>0)
            return;
        cooldownTimer=multiStackCooldown;
        RefilCrystal();

        
    }
}
