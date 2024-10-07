using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class GameData
{
    public int currency;
    public SerializableDictionary<string,bool> skillTree;
    public SerializableDictionary<string,int> inventory;
    public List<string> equipmentId;
    public SerializableDictionary<string,bool> checkPoint; 
    public string closestCheckPointId;
    public float lostCurrencyX;
    public float lostCurrencyY;
    public int lostCurrencyAmount;
    public GameData()
    {
        
        this.currency = 0;
        this.skillTree = new SerializableDictionary<string,bool>();
        this.inventory = new SerializableDictionary<string,int>();
        this.equipmentId = new List<string>();
        this.checkPoint = new SerializableDictionary<string,bool>();
        this.closestCheckPointId = string.Empty;
        this.lostCurrencyX = 0;
        this.lostCurrencyY = 0;
        this.lostCurrencyAmount = 0;
    }
}
