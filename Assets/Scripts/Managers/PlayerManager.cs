using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour, ISaveManager
{
    public static PlayerManager instance;
    public Player player;
    public int currency;
    private void Awake() 
    {
        if(instance != null)
            Destroy(instance.gameObject);
        else
            instance=this;    
            
    }

    public bool HaveEnoughMoney(int _price)
    {
        if(_price > currency)
        {
            Debug.Log("Not enough money");
            return false;
        }
        currency -= _price;
        return true;
    }
    public int GetCurrency() => currency;

    public void LoadData(GameData _data)
    {
        if(_data.currency == 0)
        {
            currency = 100000;
            return;
        }
        this.currency = _data.currency;
    }

    public void SaveGame(ref GameData _data)
    {
        _data.currency = this.currency;
    }
}
