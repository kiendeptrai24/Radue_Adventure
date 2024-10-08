using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Blackhole_hotKey_Controller : MonoBehaviour
{
    private SpriteRenderer sr;
    private KeyCode myHotkey;
    private TextMeshProUGUI myText;
    private Transform myEnemy;
    private Blackhole_Skill_Controller blackHole;

    public void SetupHotKey(KeyCode _myNewHotKey,Transform _myEnemy,Blackhole_Skill_Controller _myBlackHole)
    {
        sr= GetComponent<SpriteRenderer>();
        myText = GetComponentInChildren<TextMeshProUGUI>();


        myEnemy = _myEnemy;
        blackHole =_myBlackHole;

        myHotkey = _myNewHotKey;
        myText.text = myHotkey.ToString();
    }
    private void Update() {
        if(Input.GetKeyDown(myHotkey))
        {
            blackHole.AddEnemyToList(myEnemy);

            myText.color = Color.clear;
            sr.color = Color.clear;
        }
    }
}
