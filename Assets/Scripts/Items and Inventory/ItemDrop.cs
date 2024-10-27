using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    [SerializeField] private int maxItemsToDrop;
    [SerializeField] private ItemData[] possibleDrop;
    private List<ItemData> dropList = new List<ItemData>();


    [SerializeField] private GameObject dropPrefab;


    public virtual void GenerateDrop()
    {
        Debug.Log("drop");
        for (int i = 0; i < possibleDrop.Length; i++)
        {
            if(Random.Range(0, 100) <= possibleDrop[i].dropChance)
            {
                dropList.Add(possibleDrop[i]);
            }
        }
        if(dropList.Count <= 0)
            return;
        for (int i = 0; i < maxItemsToDrop; i++)
        {
            int ran =Random.Range(0, dropList.Count);
            ItemData randomItem = dropList[ran];

            //dropList.Remove(randomItem);
            DropItem(randomItem);
        }

    }


    protected void DropItem(ItemData _itemData)
    {
        GameObject newDrop =Instantiate(dropPrefab,transform.position,Quaternion.identity);
        Vector2 randomVelocity = new Vector2(Random.Range(-5,5),Random.Range(15,20));
        newDrop.GetComponent<ItemObject>().SetupItem(_itemData,randomVelocity);
    }
}
