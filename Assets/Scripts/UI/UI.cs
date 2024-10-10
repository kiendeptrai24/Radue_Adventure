using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI : MonoBehaviour, ISaveManager
{
    [Header("End Screen")]
    [SerializeField] private GameObject endText;
    [SerializeField] private UI_FadeScreen fadeScreen;
    [SerializeField] private GameObject restartButton;  
    [Space]
    [SerializeField] private GameObject characterUI;
    [SerializeField] private GameObject skillTreeUI;
    [SerializeField] private GameObject craftUI;
    [SerializeField] private GameObject optionsUI;
    [SerializeField] private GameObject inGameUI;


    public UI_ItemTooltip itemTooltip;
    public UI_StatToolTip statToolTip;
    public UI_CraftWindow craftWindow;
    public UI_SkillToolTip skillToolTip;

    [SerializeField] private UI_VolumeSlide[] volumeSetting;
    private void Start() {
        
        SwitchTo(inGameUI);
        itemTooltip.gameObject.SetActive(false);
        statToolTip.gameObject.SetActive(false);
    }
    private void Update() {
        if(Input.GetKeyDown(KeyCode.C))
            SwitchWithKeyTo(characterUI);
        if(Input.GetKeyDown(KeyCode.B))
            SwitchWithKeyTo(craftUI);    
        if(Input.GetKeyDown(KeyCode.K))
            SwitchWithKeyTo(skillTreeUI);
        if(Input.GetKeyDown(KeyCode.O))
            SwitchWithKeyTo(optionsUI);
    }
    public void SwitchTo(GameObject _menu)
    {
        if(_menu == null)
            return;
        for (int i = 0; i < transform.childCount; i++)
        {
            bool fadeScreen = transform.GetChild(i).GetComponent<UI_FadeScreen>() != null; // we need this to keep fade screen gameObject active
            if(fadeScreen == false)
                transform.GetChild(i).gameObject.SetActive(false);
        }

        if(_menu != null)
        {
            AudioManger.instance.PlayerSFX(7,null);
            _menu.SetActive(true);
        }

    }
    public void SwitchWithKeyTo(GameObject _menu)
    {
        
        if(_menu != null && _menu.activeSelf)
        {
            _menu.SetActive(false);
            CheckForInGameUI();
            return;
        }
        SwitchTo(_menu);
    }

    private void CheckForInGameUI()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if(transform.GetChild(i).gameObject.activeSelf && !transform.GetChild(i).GetComponent<UI_FadeScreen>())
                return;
        }
        SwitchTo(inGameUI);
    }
    public void SwitchOnEndScreen()
    {
        SwitchTo(null);
        fadeScreen.FadeOut();
        StartCoroutine(EndScreenCoroutine());

    }
    IEnumerator EndScreenCoroutine()
    {
        yield return new WaitForSeconds(1);
        endText.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        restartButton.SetActive(true);
    }
    public void RestartGameButton() => GameManager.instance.RestartScene();
    public void ExitAndSave()
    {
        StartCoroutine(nameof(Exit));
    }   
    private IEnumerator Exit()
    {
        fadeScreen.FadeOut();
        yield return new WaitForSeconds(1.3f);
        GameManager.instance.ExitAndSave();
    }

    public void LoadData(GameData _data)
    {
        foreach (KeyValuePair<string, float> pair in _data.volumeSetting)
        {
            foreach (UI_VolumeSlide item in volumeSetting)
            {
                if(item.parameter == pair.Key)
                    item.LoadSlider(pair.Value);
            }
        }
    }

    public void SaveGame(ref GameData _data)
    {
        _data.volumeSetting.Clear();

        foreach (UI_VolumeSlide item in volumeSetting)
        {
            _data.volumeSetting.Add(item.parameter,item.slider.value);
        }
    }
}
