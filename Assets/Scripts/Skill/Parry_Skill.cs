using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Parry_Skill : Skill
{
    [Header("Parry")]
    [SerializeField] private UI_SkillTreeSlot parryUnlockbutton;
    public bool parryUnlocked { get; private set; }

    [Header("Parry restore")]
    [SerializeField] private UI_SkillTreeSlot restoreUnlockButton;
    [Range(0f,1f)]
    [SerializeField] private float restorehealPersentage;
    [Header("parry with mirage")]
    [SerializeField] private float res;
    public bool restoreUnlocked { get; private set; }

    

    [Header("Parry with mirage")]
    [SerializeField] private UI_SkillTreeSlot parryWithMirageUnlockButton;
    public bool parryWithMirageUnlocked { get; private set; }





    public override void UseSkill()
    {
        base.UseSkill();
        if(restoreUnlocked)
        {
            int restoreAmount =Mathf.RoundToInt(player.stats.GetMaxHealthValue() * restorehealPersentage);
            player.stats.IncreaseHealthBy(restoreAmount);
        }
    }

    protected override void Start()
    {
        base.Start();
        parryUnlockbutton.GetComponent<Button>().onClick.AddListener(UnlockParry);
        restoreUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockParryRetore);
        parryWithMirageUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockParryWithMirage);
    }
    private void UnlockParry()
    {
        if(parryUnlockbutton.unclocked)
            parryUnlocked = true;
    }
    private void UnlockParryRetore()
    {
        if(restoreUnlockButton.unclocked)
            restoreUnlocked = true;
    }
    private void UnlockParryWithMirage()
    {
        if(parryWithMirageUnlockButton.unclocked)
            parryWithMirageUnlocked = true;
    }
    public void MakeMirageOnParry(Transform _respawnTranform)
    {
        if(parryWithMirageUnlocked)
            SkillManager.instance.clone.CreateCloneWithDelay(_respawnTranform);
    }
}
