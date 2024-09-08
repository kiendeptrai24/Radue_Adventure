using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public enum EquipmentType
{
    Weapon,
    Armor,
    Amulet,
    Flask
}
[CreateAssetMenu(fileName = "New Item Data", menuName = "Data/Equipment")]
public class ItemData_Equipment : ItemData
{
    public EquipmentType equipmentType;

    [Header("Unique effect")]
    public float itemCooldown; 
    public Item_Effect[] itemEffects;



    [Header("Major Stats")]
    public int strength; 
    public int agility; 
    public int intelligence; 
    public int vitality; 

    [Header("Offensive stats")]
    public int damage;
    public int critChance;
    public int critPower; 

    [Header("defensive stats")]
    public int maxHealth;
    public int armor;
    public int evasion;
    public int magicResistance;

    [Header("Magic stats")]
    public int fireDamage;
    public int iceDamage;
    public int lightingDamage;

    [Header("Craft requirements")]

    public List<InventoryItem> craftingMaterial;

    private int descriptionLength;

    public void Effect(Transform _enemyPosition)
    {
        foreach (var item in itemEffects)
        {
            item.ExecuteEffect(_enemyPosition);
        }
    }
    public void AddModifiers()
    {
        PlayerStats playerStats =PlayerManager.instance.player.GetComponent<PlayerStats>();

        playerStats.strength.AddModifier(strength);
        playerStats.agility.AddModifier(agility);
        playerStats.intelligence.AddModifier(intelligence);
        playerStats.vitality.AddModifier(vitality);

        playerStats.damage.AddModifier(damage);
        playerStats.critChance.AddModifier(critChance);
        playerStats.critPower.AddModifier(critPower);

        playerStats.maxHealth.AddModifier(maxHealth);
        playerStats.armor.AddModifier(armor);
        playerStats.evasion.AddModifier(evasion);
        playerStats.magicResistance.AddModifier(magicResistance);

        playerStats.fireDamage.AddModifier(fireDamage);
        playerStats.iceDamage.AddModifier(iceDamage);
        playerStats.lightingDamage.AddModifier(lightingDamage);



    }
    public void RemoveModifiers()
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();

        playerStats.strength.RemoveModifier(strength);
        playerStats.agility.RemoveModifier(agility);
        playerStats.intelligence.RemoveModifier(intelligence);
        playerStats.vitality.RemoveModifier(vitality);

        playerStats.damage.RemoveModifier(damage);
        playerStats.critChance.RemoveModifier(critChance);
        playerStats.critPower.RemoveModifier(critPower);

        playerStats.maxHealth.RemoveModifier(maxHealth);
        playerStats.armor.RemoveModifier(armor);
        playerStats.evasion.RemoveModifier(evasion);
        playerStats.magicResistance.RemoveModifier(magicResistance);

        playerStats.fireDamage.RemoveModifier(fireDamage);
        playerStats.iceDamage.RemoveModifier(iceDamage);
        playerStats.lightingDamage.RemoveModifier(lightingDamage);

    }

    public override string GetDescription()
    {
        sb.Length = 0;
        descriptionLength =0;
        AddItemDescription(strength, "Strength");
        AddItemDescription(agility, "Agility");
        AddItemDescription(intelligence, "Intelligence");
        AddItemDescription(vitality, "Vitality");

        AddItemDescription(damage, "Damage");
        AddItemDescription(critChance, "CritChance");
        AddItemDescription(critPower, "CritPower");
        AddItemDescription(maxHealth, "MaxHealth");

        AddItemDescription(armor, "Armor");
        AddItemDescription(evasion, "Evasion");
        AddItemDescription(magicResistance, "MagicResistance");

        AddItemDescription(fireDamage, "FireDamage");
        AddItemDescription(iceDamage, "IceDamage");
        AddItemDescription(iceDamage, "LightingDamage");


        if(descriptionLength < 5)
        {
            for (int i = 0; i < 5 - descriptionLength; i++)
            {
                sb.AppendLine();
                sb.Append("");
            }
        }
        for (int i = 0; i < itemEffects.Length; i++)
        {
            if(itemEffects[i].effectDescription.Length >0)
            {
                sb.AppendLine();
                sb.Append("Unique: " + itemEffects[i].effectDescription);
                descriptionLength++;
            }
        }

        return sb.ToString();
    }
    private void AddItemDescription(int _value,string _name)
    {
        if(_value != 0)
        {
            if(sb.Length == 0)
                sb.Append("Attributes:");
            
            
            if(sb.Length > 0)
                sb.AppendLine();
            
            
            if(_value > 0)
                sb.Append(" + " + _value + " " + _name);
            descriptionLength++;
        }
    }

}