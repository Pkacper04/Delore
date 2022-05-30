using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using NaughtyAttributes;

public class MenuController : MonoBehaviour
{

    [SerializeField] private Button continueButton;
    [SerializeField] private GameObject settings;
    [SerializeField] private GameObject infoBox;
    [SerializeField] private GameObject credits;
    [SerializeField] private GameObject tutorial;
    [SerializeField] private Sprite unClickableButton;
    [SerializeField] private Animator animator;

    [SerializeField, AnimatorParam("animator")]
    private string creditsParam;

    [Scene]
    public string newGameScene;
    private int levelIndex;

    private void Start()
    {
        settings.SetActive(false);
        infoBox.SetActive(false);
        credits.SetActive(false);
        tutorial.SetActive(false);
        PlayerData data = SaveSystem.LoadPlayer();
        if (data == null)
        {
            continueButton.interactable = false;
            continueButton.GetComponent<Animator>().enabled = false;
            continueButton.GetComponent<Image>().sprite = unClickableButton;
        }
        else
            levelIndex = data.levelID;
        
    }
    public void Continue()
    {
        SceneManager.LoadScene(levelIndex-1);
    }
    public void NewGame()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsTag("buttons"))
            return;
        if (animator.IsInTransition(0))
            return;
        if (continueButton.interactable == false)
            StartNewGame();

        if (infoBox.activeInHierarchy)
        {
            animator.SetBool("info", false);
            StartCoroutine("WaitForAnimation", infoBox);
        }
        else
        {
            animator.SetBool("info", true);
            infoBox.SetActive(true);
        }

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (credits.activeInHierarchy)
            {
                Credits();
            }
        }
    }

    public void StartNewGame()
    {
        SaveSystem.DeletePlayerSave();
        SceneManager.LoadScene(newGameScene);
    }

    public void Settings()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsTag("buttons"))
            return;
        if (animator.IsInTransition(0))
            return;
        if (infoBox.activeInHierarchy)
            return;
        if (credits.activeInHierarchy)
            return;

        if (settings.activeInHierarchy)
        {
            animator.SetBool("settings", false);
            StartCoroutine("WaitForAnimation",settings);
        }
        else
        {
            animator.SetBool("settings", true);
            settings.SetActive(true);
        }

    }

    public void Credits()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsTag("buttons"))
            return;
        if (animator.IsInTransition(0))
            return;
        if (infoBox.activeInHierarchy)
            return;
        if (settings.activeInHierarchy)
            return;

        if (credits.activeInHierarchy)
        {
            animator.SetBool(creditsParam, false);
            StartCoroutine("WaitForAnimation", credits);
        }
        else
        {
            animator.SetBool(creditsParam, true);
            credits.SetActive(true);
        }
    }

    public void Tutorial()
    {
        if(tutorial.activeInHierarchy)
        {
            tutorial.SetActive(false);
            settings.SetActive(true);
        }
        else
        {
            tutorial.SetActive(true);
            settings.SetActive(false);
        }
    }
    public void Exit()
    {
        Application.Quit();
    }

    public IEnumerator WaitForAnimation(GameObject panel)
    {
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsTag("end") == true);
        panel.SetActive(false);
    }

}
