using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class ItemObject : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private ItemData itemData;
    private ItemObject_Trigger itemObject_Trigger;

    private void Awake() {
        itemObject_Trigger = GetComponentInChildren<ItemObject_Trigger>();
    }
    private void OnValidate()
    {
        SetupVisuals();
    }

    private void SetupVisuals()
    {
        if (itemData == null)
            return;
        GetComponent<SpriteRenderer>().sprite = itemData.itemIcon;
        gameObject.name = "Item objext - " + itemData.name;
    }
    public void SetupItem(ItemData _itemData,Vector2 _velocity)
    {
        itemData = _itemData;
        rb.velocity =_velocity;
        SetupVisuals();
    }
    

    public void PickUpItem()
    {
        if(!Inventory.instance.CanAddItem() && itemData.itemType == ItemType.Equipment)
        {
            rb.velocity = new Vector2(0,5);
            PlayerManager.instance.player.fx.CreatePupUpText("Inventory is full"); 
            return;
        }
        AudioManger.instance.PlayerSFX(18,transform);    
        Inventory.instance.AddItem(itemData);
        Destroy(gameObject);
    }


}
