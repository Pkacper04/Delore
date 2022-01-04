using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseController : MonoBehaviour
{
    public static bool GamePaused = false;
    [SerializeField] private GameObject PausePanel;


    private void Start()
    {
        PausePanel.SetActive(false);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E)) // do zmiany na esc po wsyzstkich testach
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
            PausePanel.SetActive(true);
        }
        else
        {
            Time.timeScale = 1f;
            PausePanel.SetActive(false);
        }
    }


    public void ResumeGame()
    {
        GamePaused = false;
        PauseGame();
    }

    public void ExitGame()
    {
        Debug.Log("Wyjscie z gry");
        //SceneManager.LoadScene("Menu");
        //Doko�czy� gdy b�dzie gotowa scena menu
    }

    public void Settings()
    {
        Debug.Log("Settings on");
        //Doda� uruchamianie ustawie� w przysz�o�ci
    }

    public void Save()
    {
        Debug.Log("Saved Game");
        //Zapisywanie po kliknieciu stanu gry te� w przysz�o�ci
    }

}
