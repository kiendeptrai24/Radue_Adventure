using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.UI;

public class Dodge_Skill : Skill
{
    [Header("Dodge")]
    [SerializeField] private UI_SkillTreeSlot unlockDodgeButton;
    [SerializeField] private int evasionAmount;
    public bool dodgedUnlock;

    [Header("Mirage dodge")]
    [SerializeField] private UI_SkillTreeSlot unlockMirageDodgeButton;
    public bool dodgeMirageUnloced;

    protected override void Start()
    {
        base.Start();
        
    }
    protected override void AddButtonSkillTree()
    {
        unlockDodgeButton.GetComponent<Button>().onClick.AddListener(UnlockDodge);
        unlockMirageDodgeButton.GetComponent<Button>().onClick.AddListener(UnlockMirageDodge);
    }  
    protected override void CheckUnlock()
    {
        UnlockDodge();
        UnlockMirageDodge();
    }
    private void UnlockDodge()
    {
        if(unlockDodgeButton.unlocked && !dodgedUnlock)
        {
            player.stats.evasion.AddModifier(evasionAmount);
            Inventory.instance.UpdateStatsUI();
            dodgedUnlock = true;

        }
    }
    private void UnlockMirageDodge()
    {
        if(unlockMirageDodgeButton)
            dodgeMirageUnloced = true;
    }
    public void CreateMirageOnDodge()
    {
        if(dodgeMirageUnloced)
            SkillManager.instance.clone.CreateClone(player.transform,new Vector3(2 * player.facingDir,0));
    }
}
