using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class TutorialEnable : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup tutorial;

    [SerializeField]
    private Animator tutorialAnimator;

    [SerializeField, AnimatorParam("tutorialAnimator")]
    private string tutorialParam;

    [SerializeField]
    private float delay;


    private void Start()
    {
        tutorial.alpha = 0;
        tutorial.blocksRaycasts = false;
        tutorial.interactable = false;
        StartCoroutine(ShowTutorialWithDelay(delay));
    }


    public void Tutorial()
    {
        if (tutorialAnimator.GetBool(tutorialParam))
        {
            tutorialAnimator.SetBool(tutorialParam, false);
            StartCoroutine(WaitForAnimator());
        }
    }
    private IEnumerator ShowTutorialWithDelay(float delay)
    {
        PauseController.GamePaused = true;
        PauseController.BlockPauseMenu = true;
        yield return new WaitForSecondsRealtime(delay);
        tutorialAnimator.SetBool(tutorialParam, true);
        tutorial.alpha = 1;
        tutorial.blocksRaycasts = true;
        tutorial.interactable = true;
    }

    private IEnumerator WaitForAnimator()
    {
        yield return new WaitForSecondsRealtime(1f);
        tutorial.alpha = 0;
        tutorial.blocksRaycasts = false;
        tutorial.interactable = false;
        PauseController.BlockPauseMenu = false;
        PauseController.GamePaused = false;
    }
}
