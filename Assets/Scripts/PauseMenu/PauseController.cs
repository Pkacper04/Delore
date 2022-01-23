using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseController : MonoBehaviour
{
    public static bool GamePaused = false;
    [SerializeField] private Canvas canvas;


    private void Start()
    {
        canvas.enabled = false;
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
            canvas.enabled = true;
        }
        else
        {
            Time.timeScale = 1f;
            canvas.enabled = false;
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
        SaveSystem.SavePlayer(GameObject.FindGameObjectWithTag("Player"));
    }

}
