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
        SaveSystem.SavePlayerExit(GameObject.FindGameObjectWithTag("Player"));
        GamePaused = false;
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
    public void ExitGame()
    {
        SaveSystem.SavePlayerExit(GameObject.FindGameObjectWithTag("Player"));
        Application.Quit();
    }

    public void Settings()
    {
        settings.SetActive(!settings.activeInHierarchy);
    }

    public void Save()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if(!player.GetComponent<Movement>().crouch)
            SaveSystem.SavePlayer(player);
    }

}
