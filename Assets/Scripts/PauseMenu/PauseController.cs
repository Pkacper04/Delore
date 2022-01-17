using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseController : MonoBehaviour
{
    public static bool GamePaused = false;
    [SerializeField] private GameObject Pause;




    private void Start()
    {
        Pause.SetActive(false);
    }
    
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            GamePaused = !GamePaused;
            PauseGame();
        }
    }

    private void PauseGame()
    {
        if(GamePaused)
        {
            Time.timeScale = 0f;
            Pause.SetActive(true);
        }
        else
        {
            Time.timeScale = 1f;
            Pause.SetActive(false);
        }
    }


    public void ResumeGame()
    {
        GamePaused = false;
        PauseGame();
    }


    public void ExitToMenu()
    {
        GamePaused = false;
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
    public void ExitGame()
    {
        Application.Quit();
    }

    public void Settings()
    {
        Debug.Log("Settings on");
        //Dodaæ uruchamianie ustawieñ w przysz³oœci
    }

    public void Save()
    {
        Debug.Log("Saved Game");
        SaveSystem.SavePlayer(GameObject.FindGameObjectWithTag("Player"));
    }

}
