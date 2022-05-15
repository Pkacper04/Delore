using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;

public class CreditsController : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup nextButton;
    [SerializeField]
    private CanvasGroup backButton;
    [SerializeField]
    private Image creditPage;
    [ReorderableList]
    public List<Sprite> creditsPages;
    

    private int currentPage = 0;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (currentPage != 0)
                GoBack();
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (currentPage != 2)
                GoNext();
        }
    }

    public void Enable()
    {
        switch (currentPage)
        {
            case 0:
                ActivateButton(nextButton);
                DisableButton(backButton);
                break;
            case 1:
                ActivateButton(nextButton);
                ActivateButton(backButton);
                break;
            case 2:
                DisableButton(nextButton);
                ActivateButton(backButton);
                break;
        }
    }

    private void ActivateButton(CanvasGroup button)
    {
        button.alpha = 1;
        button.blocksRaycasts = true;
        button.interactable = true;
    }

    private void DisableButton(CanvasGroup button)
    {
        button.alpha = 0;
        button.blocksRaycasts = false;
        button.interactable = false;
    }

    private void ChangePage()
    {
        creditPage.sprite = creditsPages[currentPage];
        Enable();
    }

    public void GoNext()
    {
        currentPage++;
        ChangePage();
    }
    public void GoBack()
    {
        currentPage--;
        ChangePage();
    }

}
