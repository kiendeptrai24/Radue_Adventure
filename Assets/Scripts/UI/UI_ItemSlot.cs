using UnityEngine.UI;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_ItemSlot : MonoBehaviour , IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    protected UI ui;
    public InventoryItem item;
    [SerializeField] protected Image itemImage;
    [SerializeField] protected TextMeshProUGUI itemText;

 
    protected virtual void Start() {
        ui = GetComponentInParent<UI>();
    }
    public void UpdateSlot(InventoryItem _newItem)
    {
        item =_newItem;
        itemImage.color = Color.white;
        itemImage.sprite = item.data.itemIcon;
        if (item.stackSize > 1)
        {
            itemText.text = item.stackSize.ToString();
        }
        else
        {
            itemText.text = "";
        }
        
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        if(item == null)
            return;
        if(Input.GetKey(KeyCode.LeftControl))
        {
            Inventory.instance.RemoveItem(item.data);
            return;
        }
        if(item.data.itemType  == ItemType.Equipment)
            Inventory.instance.EquipItem(item.data);
        ui.itemTooltip.HideTooltip();
    }
    public void CleanUpSlot()
    {
        item = null;
        itemImage.sprite = null;
        itemImage.color = Color.clear;

        itemText.text="";
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(item == null || item.data == null)
            return;
   

        ui.itemTooltip.ShowTooltip(item.data as ItemData_Equipment);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ui.itemTooltip.HideTooltip();
    }
}
