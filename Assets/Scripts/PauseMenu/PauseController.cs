using UnityEngine;
using UnityEngine.SceneManagement;
using Delore.Player;

public class PauseController : MonoBehaviour
{
    public static bool GamePaused = false;
    public static bool GameEnded = false;
    [SerializeField] private Canvas canvas;
    [SerializeField] private GameObject settings;


    private void Start()
    {
        Time.timeScale = 1;
        canvas.enabled = false;
        settings.SetActive(false);
        GameEnded = false;
        GamePaused = false;
    }
    
    void Update()
    {
        if(!GameEnded && Input.GetKeyDown(KeyCode.Escape))
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

}
