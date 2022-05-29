using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CreditsController : MonoBehaviour
{
    [SerializeField]
    private List<Sprite> credits = new List<Sprite>();
    [SerializeField]
    private Image creditsContainer;

    [SerializeField]
    private CanvasGroup rightButton;
    [SerializeField]
    private CanvasGroup leftButton;
    
    private int panelId;

    private void UpdateSprite()
    {
        creditsContainer.sprite = credits[panelId];
        if(panelId == 0)
        {
            DisableLeftButton();
            EnableRightButton();
        }
        if(panelId == 1)
        {
            EnableLeftButton();
            EnableRightButton();
        }
        if(panelId == 2)
        {
            EnableLeftButton();
            DisableRightButton();
        }
    }

    public void SkipRight()
    {
        panelId++;
        UpdateSprite();
    }

    public void SkipLeft()
    {
        panelId--;
        UpdateSprite();
    }


    private void DisableRightButton()
    {
        rightButton.alpha = 0;
        rightButton.interactable = false;
        rightButton.blocksRaycasts = false;
    }

    private void DisableLeftButton()
    {
        leftButton.alpha = 0;
        leftButton.interactable = false;
        leftButton.blocksRaycasts = false;
    }

    private void EnableRightButton()
    {
        rightButton.alpha = 1;
        rightButton.interactable = true;
        rightButton.blocksRaycasts = true;
    }

    private void EnableLeftButton()
    {
        leftButton.alpha = 1;
        leftButton.interactable = true;
        leftButton.blocksRaycasts = true;
    }

}
