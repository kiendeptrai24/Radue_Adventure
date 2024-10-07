using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class SaveManager : MonoBehaviour
{

    public static SaveManager instance;
    [SerializeField] private string fileName;
    [SerializeField] private bool encryptData;
    private GameData gameData;
    private List<ISaveManager> saveManagers= new List<ISaveManager>();
    
    private FileDataHandler dataHandler;
    [ContextMenu("Delete save file")]
    public void DeleteSaveData()
    {
        dataHandler = new FileDataHandler(Application.persistentDataPath ,fileName,encryptData);
        dataHandler.Delete();
    }
    private void Awake() 
    {
        if(instance == null)
            instance = this;
        else
            Destroy(instance.gameObject);   
    }

    private void Start() {
        dataHandler = new FileDataHandler(Application.persistentDataPath,fileName,encryptData);
        saveManagers = FindAllSaveManagers();
        LoadGame();
    }
    public void NewGame()
    {
        gameData = new GameData();
    }

    public void LoadGame()
    {
        gameData = dataHandler.Load();
        if(this.gameData == null)
        {
            Debug.Log("No saved data found!");
            NewGame();
        }

        foreach (ISaveManager saveManager in saveManagers)
        {
            saveManager.LoadData(gameData);
        }
        Debug.Log("loaded currency " + gameData.currency);
    }
    
    public void SaveGame()
    {
        foreach(ISaveManager saveManager in saveManagers)
        {
            saveManager.SaveGame(ref gameData);
        }
        dataHandler.Save(gameData);
        Debug.Log("Save currency " + gameData.currency);
    }
    private void OnApplicationQuit() 
    {
        SaveGame();
    }
    private List<ISaveManager> FindAllSaveManagers()
    {
        IEnumerable<ISaveManager> saveManagers = Resources.FindObjectsOfTypeAll<MonoBehaviour>().OfType<ISaveManager>();
        
        
        return new List<ISaveManager>(saveManagers);
    }



    public bool HadSaveData()
    {
        
        if(dataHandler.Load() != null)
            return true;
        return false;
    }
}
