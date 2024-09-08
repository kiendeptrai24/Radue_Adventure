using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.UI;

public class CloneSkill : Skill
{
    [Header("Clone info")]
    [SerializeField] private float attackMultiplier;
    [SerializeField] private GameObject clonePrefabs;
    [SerializeField] private float cloneDuration;
    [Space]
    [Header("Clone attack")]
    [SerializeField] private UI_SkillTreeSlot cloneAttackUnlockButton;
    [SerializeField] private float cloneAttackMultiplier;
    [SerializeField] private bool canAttack;

    [Header("Aggresive clone")]
    [SerializeField] private UI_SkillTreeSlot aggresiveCloneUnlockButton;
    [SerializeField] private float aggresiveCloneAttackMultiplier;
    public bool canApplyOnHitEffect { get; private set; }

    [Header("Multiple clone")]
    [SerializeField] private UI_SkillTreeSlot multipleUnlockButton;
    [SerializeField] private float multiCloneAttackMultiplier;
    [SerializeField] private bool canDuplicateClone;
    [SerializeField] private float chanceToDuplicate;
    [Header("Crystal instead of clone")]
    [SerializeField] private UI_SkillTreeSlot crystalInsteadUnlockButton;
    public bool crystalInsteadOfClone;


    protected override void Start()
    {
        base.Start();

        cloneAttackUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockCloneAttack);
        aggresiveCloneUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockAggresiveClone);
        multipleUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockMultiClone);
        crystalInsteadUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockCrystalInstead);

    }


    #region Unlock Region

    private void UnlockCloneAttack()
    {
        if(cloneAttackUnlockButton.unclocked)
        {
            canAttack = true;
            attackMultiplier =cloneAttackMultiplier;
        }
    }
    private void UnlockAggresiveClone()
    {
        if(aggresiveCloneUnlockButton.unclocked)
        {
            canApplyOnHitEffect = true;
            attackMultiplier = multiCloneAttackMultiplier;
        }
    }
    private void UnlockMultiClone()
    {
        if(multipleUnlockButton.unclocked)
        {
            canDuplicateClone = true;
            attackMultiplier = multiCloneAttackMultiplier;
        }
    }
    private void UnlockCrystalInstead()
    {
        if(crystalInsteadUnlockButton.unclocked)
        {
            crystalInsteadOfClone = true;
        }
    }






    #endregion
   
    public void CreateClone(Transform _clonePosition,Vector3 _offset)
    {
        //new
        if(crystalInsteadOfClone)
        {
            SkillManager.instance.crystal.CreateCrystal();
            return;
        }
        GameObject newClone= Instantiate(clonePrefabs);
        newClone.GetComponent<CloneSkillController>().
        SetUpClone(_clonePosition,cloneDuration,canAttack,_offset,FindClosestEnnemy(newClone.transform),canDuplicateClone,chanceToDuplicate,player, attackMultiplier);
    }
    
    public void CreateCloneWithDelay(Transform _enemyTransform)
    {

        StartCoroutine(CloneDelayCoroutine(_enemyTransform,new Vector3(1.5f * player.facingDir,0)));
    }
    private IEnumerator CloneDelayCoroutine(Transform _enemyTransform,Vector3 _offset)
    {
        yield return new WaitForSeconds(.4f);
        CreateClone(_enemyTransform, _offset);
    }
}
