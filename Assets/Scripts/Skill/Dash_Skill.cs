using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class Dash_Skill : Skill
{
    [Header("Dash")]
    [SerializeField] private UI_SkillTreeSlot dashUnlockButton;
    public bool dashUnlocked { get; private set; }
    [Header("Clone on dash")]
    [SerializeField] private UI_SkillTreeSlot cloneOnDashUnlockButton;
    public bool cloneOnDashUnlocled{ get; private set; }

    [Header("Clone on arrival")]
    [SerializeField] private UI_SkillTreeSlot cloneOnArrivalUnlockButton;
    public bool cloneOnArrivalUnlocked{ get; private set; }

    public override void UseSkill()
    {
        base.UseSkill();
    }
    protected override void Start()
    {
        base.Start();
        dashUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockDash);
        cloneOnDashUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockCloneOnDash);
        cloneOnArrivalUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockCloneOnArrival);
    }
    private void UnlockDash()
    {
        if(dashUnlockButton.unclocked)
            dashUnlocked = true;
    }
    private void UnlockCloneOnDash()
    {
        if(cloneOnDashUnlockButton.unclocked)
            cloneOnDashUnlocled = true;
    }
    private void UnlockCloneOnArrival()
    {
        if(cloneOnArrivalUnlockButton.unclocked)
            cloneOnArrivalUnlocked = true;
    }
    public void cloneOnDash()
    {
        if(cloneOnDashUnlocled)
            SkillManager.instance.clone.CreateClone(player.transform,Vector3.zero);
    }
    public void CloneOnArrival()
    {
        if(cloneOnArrivalUnlocked)
            SkillManager.instance.clone.CreateClone(player.transform,Vector3.zero);
    }
}
