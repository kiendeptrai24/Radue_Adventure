using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class Blackhole_Skill : Skill
{
    [SerializeField] private UI_SkillTreeSlot blackHoleUnlockButton;
    public bool blackholeUnlocked { get; private set; }
    [SerializeField] private int amountOfAttacks;
    [SerializeField] private float cloneCooldown;
    [SerializeField] private float blackholeDuration;
    [Space]
    [SerializeField] private GameObject blackHolePrefab;
    [SerializeField] private float maxSize;
    [SerializeField] private float growSpeed;
    [SerializeField] private float shrinkSpeed;
    Blackhole_Skill_Controller currentBlackHole;


    

    protected override void Start() {
        base.Start();
        blackHoleUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockBlackHole);
    }
    private void UnlockBlackHole()
    {
        if(blackHoleUnlockButton.unlocked)
            blackholeUnlocked = true;
    }
    protected override void Update()
    {
        base.Update();
    }
    public override bool CanUseSkill()
    {
        return base.CanUseSkill();
    }
    public override void UseSkill()
    {
        base.UseSkill();
        GameObject newBlackHole = Instantiate(blackHolePrefab,player.transform.position,Quaternion.identity);
        currentBlackHole= newBlackHole.GetComponent<Blackhole_Skill_Controller>();
        currentBlackHole.SetupBlackhole(maxSize, growSpeed, shrinkSpeed,amountOfAttacks, cloneCooldown, blackholeDuration);
        AudioManger.instance.PlayerSFX(3,null);
        AudioManger.instance.PlayerSFX(6,null);

    }
    public bool SkillCompleted()
    {
        if(!currentBlackHole)
            return false;
        if(currentBlackHole.playerCanExitState)
        {
            currentBlackHole=null;
            return true;
        }
        return false;
    }
    //new
    public float GetBlackholeRadios()
    {
        return maxSize/2;
    }
    protected override void CheckUnlock()
    {
        UnlockBlackHole();
    }
}
