using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_SkillTreeSlot : MonoBehaviour ,IPointerEnterHandler, IPointerExitHandler, ISaveManager
{
    private UI ui;
    private Image skillImage;

    [SerializeField] private int skillCost;
    [SerializeField] private string skillName;
    [TextArea]
    [SerializeField] private string skillDescription;
    [SerializeField] private Sprite defualtSkillImage;

    public bool unlocked;

    [SerializeField] private UI_SkillTreeSlot[] shouldBeUnlocked;
    [SerializeField] private UI_SkillTreeSlot[] shouldBeLocked;
    [SerializeField] private Sprite lockedSkillImage;
    [SerializeField] private Color lockColor;
    public bool checkLoad;


    private void OnValidate() 
    {
        gameObject.name ="skillTreeSlot_UI - " + skillName;
        AddListener();
    }
    private void Awake() {
        GetComponent<Button>().onClick.RemoveAllListeners();
        AddListener();
    }
    public void AddListener()
    {
        GetComponent<Button>().onClick.AddListener(() => UnclockSkillSlot());
    }

    private void Start()
    {   
        if(!ui)
            SetupComponent();
    }
    private void SetupComponent()
    {
        ui = GetComponentInParent<UI>();
        skillImage = GetComponent<Image>();
        defualtSkillImage = skillImage.sprite;
        skillImage.color = lockColor;
        skillImage.sprite = lockedSkillImage;
    }

    private void SetupInfo()
    {

        if (unlocked)
        {
            if(skillImage == null)
            {
                SetupComponent();
            }
            skillImage.color = Color.white;
            skillImage.sprite = defualtSkillImage;
        }
    }

    private void UnclockSkillSlot() 
    {
        if(PlayerManager.instance.HaveEnoughMoney(skillCost) == false)
            return;
        
        for (int i = 0; i < shouldBeUnlocked.Length; i++)
        {
            if(shouldBeUnlocked[i].unlocked == false)
            {
                Debug.Log("Cannot unlock skill");
                return;
            }
        }

        for (int i = 0; i < shouldBeLocked.Length; i++)
        {
            if(shouldBeLocked[i].unlocked == true)
            {
                Debug.Log("Cannot un lock skill");
                return;
            }
        }

        unlocked = true;
        SetupInfo();

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ui.skillToolTip.ShowToolTip(skillDescription,skillName,skillCost);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ui.skillToolTip.HideToolTip();
    }

    public void LoadData(GameData _data)
    {
        if(_data.skillTree.TryGetValue(skillName, out bool value))
        {
            unlocked = value;
            SetupInfo();
        }
    }

    public void SaveGame(ref GameData _data)
    {
        if(_data.skillTree.TryGetValue(skillName, out bool value))
        {
            _data.skillTree.Remove(skillName);
            _data.skillTree.Add(skillName, unlocked);
        }
        else
        {
            _data.skillTree.Add(skillName, unlocked);
        }
    }
}
