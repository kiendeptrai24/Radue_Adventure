
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour, ISaveManager
{
    public static GameManager instance { get; private set;}
    private Transform player;
    [SerializeField] private CheckPoint[] checkPoints;
    [Header("Lost currency")]
    [SerializeField] private GameObject lostCurrencyPrefabs;
    public int lostCurrencyAmount;
    [SerializeField] private float lostCurrencyX;
    [SerializeField] private float lostCurrencyY;

    private void Awake() 
    {
        if(instance != null)
            Destroy(instance.gameObject);
        else
            instance=this;    
        checkPoints = FindObjectsOfType<CheckPoint>();
    }
    private void Start() 
    {
        player = PlayerManager.instance.player.transform;
    }
    public void RestartScene()
    {
        SaveManager.instance.SaveGame();
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }
    public void ExitAndSave()
    {
        SaveManager.instance.SaveGame();
        SceneManager.LoadScene("MainMenu");

    }

    public void LoadData(GameData _data) => StartCoroutine(LoadWithDelay(_data));
    

    private void LoadCheckPoint(GameData _data)
    {
        foreach (KeyValuePair<string, bool> pair in _data.checkPoint)
        {
            foreach (CheckPoint checkPoint in checkPoints)
            {

                if (checkPoint.id == pair.Key && pair.Value)
                {
                    checkPoint.ActivateCheckPoint();
                }
            }
        }
        foreach (CheckPoint checkPoint in checkPoints)
        {
            if (_data.closestCheckPointId == checkPoint.id)
                PlayerManager.instance.player.transform.position = checkPoint.transform.position;
        }
    }

    private void LoadLostCurrency(GameData _data)
    {
        lostCurrencyAmount = _data.lostCurrencyAmount;
        lostCurrencyX = _data.lostCurrencyX;
        lostCurrencyY = _data.lostCurrencyY;
        if(lostCurrencyAmount > 0)
        {
            GameObject newLostCurrency = Instantiate(lostCurrencyPrefabs, new Vector3(lostCurrencyX,lostCurrencyY),Quaternion.identity);
            newLostCurrency.GetComponent<LostCurrencyController>().currency = lostCurrencyAmount;
        }
        lostCurrencyAmount =0;
    }
    private IEnumerator LoadWithDelay(GameData _data)
    {

        yield return new WaitForSeconds(.1f);
        LoadLostCurrency(_data);
        LoadCheckPoint(_data);

    }
    public void SaveGame(ref GameData _data)
    {
        _data.lostCurrencyAmount = lostCurrencyAmount;
        _data.lostCurrencyX = player.position.x;
        _data.lostCurrencyY = player.position.y;
        if(FindClosestCheckPoint() != null) 
            _data.closestCheckPointId = FindClosestCheckPoint().id;
        _data.checkPoint.Clear();
        foreach(CheckPoint checkPoint in checkPoints)
        {
            _data.checkPoint.Add(checkPoint.id,checkPoint.activationStatus);
        }
    }
    private CheckPoint FindClosestCheckPoint()
    {
        float closestDistance = Mathf.Infinity;
        CheckPoint closestCheckPoint = null;
        foreach (CheckPoint checkPoint in checkPoints)
        {
            float distanceToCheckPoint =Vector2.Distance(player.position, checkPoint.transform.position);
            if(distanceToCheckPoint < closestDistance && checkPoint.activationStatus)
            {
                closestDistance = distanceToCheckPoint;
                closestCheckPoint = checkPoint;
            }
        }
        return closestCheckPoint;
    }

    public void PauseGame(bool _pause)
    {
        if(_pause)
            Time.timeScale = 0;
        else
            Time.timeScale = 1;
    }
}
