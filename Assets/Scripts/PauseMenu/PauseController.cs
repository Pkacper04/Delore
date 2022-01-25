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
    [SerializeField] private GameObject settings;


    private void Start()
    {
        canvas.enabled = false;
        settings.SetActive(false);
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
            settings.SetActive(false);
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
        settings.SetActive(!settings.activeInHierarchy);
    }

    public void Save()
    {
        SaveSystem.SavePlayer(GameObject.FindGameObjectWithTag("Player"));
    }

}
