using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{

    [SerializeField] private Button continueButton;
    [SerializeField] private GameObject settings;

    private int levelIndex;

    private void Start()
    {
        settings.SetActive(false);
        PlayerData data = SaveSystem.LoadPlayer();
        if (data == null)
            continueButton.interactable = false;
        else
            levelIndex = data.levelID;
        
    }
    public void Continue()
    {
        SceneManager.LoadScene(levelIndex-1);
    }
    public void NewGame()
    {
        SaveSystem.DeletePlayerSave();
        SceneManager.LoadScene(1);
    }

    public void Settings()
    {
        settings.SetActive(!settings.activeInHierarchy);
    }

    public void Credists()
    {
        // Przyszle creditsy
    }

    public void Exit()
    {
        Application.Quit();
    }


}
