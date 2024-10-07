using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_MainMenu : MonoBehaviour
{
    [SerializeField] private string sceneName = "MainScene";
    [SerializeField] private GameObject continueButon;
    [SerializeField] private UI_FadeScreen fadeScreen;
    

    private void Start() {
        
        Invoke(nameof(SetContinueButton),0.1f);
        
    }
    private void SetContinueButton()
    {
        if(!SaveManager.instance.HadSaveData())
            continueButon.SetActive(false);
    }
    public void ContinueGame()
    {
        StartCoroutine(LoadSceneWithFadeEffect(1.5f));

    }
    public void NewGame()
    {
        SaveManager.instance.DeleteSaveData();
        StartCoroutine(LoadSceneWithFadeEffect(1.5f));
    }
    public void ExitGame()
    {
        Debug.Log("ExitGame");
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false; // Stop the editor
        #else
            Application.Quit(); // Quit the application
        #endif
    }
    IEnumerator LoadSceneWithFadeEffect(float _delay)
    {
        fadeScreen.FadeOut();
        yield return new WaitForSeconds(_delay);
        SceneManager.LoadScene(sceneName);
    }
}
