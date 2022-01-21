using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{

    [SerializeField] private Button continueButton;

    private void Start()
    {
        PlayerData data = SaveSystem.LoadPlayer();
        if(data == null)
            continueButton.interactable = false;
    }
    public void Continue()
    {
        SceneManager.LoadScene(1);
    }
    public void NewGame()
    {
        SaveSystem.DeletePlayerSave();
        SceneManager.LoadScene(1);
    }

    public void Settings()
    {
        //ustawienia jak olechowski skonczy
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
