using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseController : MonoBehaviour
{
    public static bool GamePaused = false;
    [SerializeField] private GameObject PausePanel;
    [SerializeField] private GameObject PauseBackground;


    private void Start()
    {
        PausePanel.SetActive(false);
        PauseBackground.SetActive(false);
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
            PauseBackground.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Time.timeScale = 1f;
            PausePanel.SetActive(false);
            PauseBackground.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
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
        //Dokoñczyæ gdy bêdzie gotowa scena menu
    }

    public void Settings()
    {
        Debug.Log("Settings on");
        //Dodaæ uruchamianie ustawieñ w przysz³oœci
    }

    public void Save()
    {
        Debug.Log("Saved Game");
        //Zapisywanie po kliknieciu stanu gry te¿ w przysz³oœci
    }

}
