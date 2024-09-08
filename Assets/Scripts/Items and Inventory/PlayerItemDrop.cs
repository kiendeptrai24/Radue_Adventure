using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerItemDrop : ItemDrop
{
    [Header("Player's drop")]
    [SerializeField] private float chanceToLooseItem;
    [SerializeField] private float chanceToLooseMaterials;

    public override void GenerateDrop()
    {
        Inventory inventory = Inventory.instance;


        List<InventoryItem> itemToUnequip = new List<InventoryItem>();
        List<InventoryItem> materialsToLoose = new List<InventoryItem>();


        foreach (InventoryItem item in inventory.GetEquipmentList())
        {
            if(Random.Range(0,100) <= chanceToLooseItem)
            {
                DropItem(item.data);
                itemToUnequip.Add(item);
            }
        }
        for (int i = 0; i < itemToUnequip.Count; i++)
        {
            inventory.UnEquipItem(itemToUnequip[i].data as ItemData_Equipment);
        }

        foreach (InventoryItem item in inventory.GetStashList())
        {
            if(Random.Range(0,100) <= chanceToLooseMaterials)
            {
                DropItem(item.data);
                materialsToLoose.Add(item);
            }
            
        }

        for (int i = 0; i < materialsToLoose.Count; i++)
        {
            inventory.RemoveItem(materialsToLoose[i].data);
        }
    }
}
