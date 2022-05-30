using UnityEngine;
using UnityEngine.SceneManagement;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;

public class PauseController : MonoBehaviour
{
    public static bool GamePaused = false;
    public static bool GameEnded = false;
    public static bool BlockPauseMenu = false;


    [SerializeField] private Canvas canvas;
    [SerializeField] private GameObject settings;
    [SerializeField] private GameObject tutorial;

    private void Awake()
    {
        Time.timeScale = 1;
        canvas.enabled = false;
        settings.SetActive(false);
        tutorial.SetActive(false);
        GameEnded = false;
        GamePaused = false;
    }


    void Update()
    {
        if(!GameEnded && !BlockPauseMenu && Input.GetKeyDown(KeyCode.Escape))
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
            tutorial.SetActive(false);
        }
    }


    public void ResumeGame()
    {
        GamePaused = false;
        PauseGame();
    }


    public void ExitToMenu()
    {
        Time.timeScale = 1;
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

    public void Tutorial()
    {
        Settings();
        tutorial.SetActive(!tutorial.activeInHierarchy);
    }


}
