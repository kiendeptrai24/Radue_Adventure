using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_SkillTreeSlot : MonoBehaviour ,IPointerEnterHandler, IPointerExitHandler
{
    private UI ui;
    private Image skillImage;

    [SerializeField] private int skillCost;
    [SerializeField] private string skillName;
    [TextArea]
    [SerializeField] private string skillDescription;
    [SerializeField] private Sprite defualtSkillImage;

    public bool unclocked;

    [SerializeField] private UI_SkillTreeSlot[] shouldBeUnlocked;
    [SerializeField] private UI_SkillTreeSlot[] shouldBeLocked;
    [SerializeField] private Sprite lockedSkillImage;
    [SerializeField] private Color lockColor;

    private void OnValidate() 
    {
        gameObject.name ="skillTreeSlot_UI - " + skillName;
    }
    private void Awake() {
        GetComponent<Button>().onClick.AddListener(() => UnclockSkillSlot());
    }

    private void Start() {
        ui = GetComponentInParent<UI>();
        skillImage = GetComponent<Image>();
        defualtSkillImage = skillImage.sprite;
        skillImage.color = lockColor;
        skillImage.sprite = lockedSkillImage;
    }

    private void UnclockSkillSlot() 
    {
        if(PlayerManager.instance.HaveEnoughMoney(skillCost) == false)
            return;
        
        for (int i = 0; i < shouldBeUnlocked.Length; i++)
        {
            if(shouldBeUnlocked[i].unclocked == false)
            {
                Debug.Log("Cannot unlock skill");
                return;
            }
        }

        for (int i = 0; i < shouldBeLocked.Length; i++)
        {
            if(shouldBeLocked[i].unclocked == true)
            {
                Debug.Log("Cannot un lock skill");
                return;
            }
        }

        unclocked = true;
        skillImage.color = Color.white;
        skillImage.sprite = defualtSkillImage;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ui.skillToolTip.ShowToolTip(skillDescription,skillName,skillCost ,unclocked);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ui.skillToolTip.HideToolTip();
    }
}
