using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Delore.Player;

public class EndGame : MonoBehaviour
{
    [SerializeField]
    private Movement movement;
    [SerializeField]
    private UIManager uiManager;
    [SerializeField]
    private Vector3 endPosition;
    [SerializeField]
    private float walkingSpeed;


    public void GameEnd()
    {
        StartCoroutine(Game());
    }

    private IEnumerator Game()
    {
        movement.end = true;
        PauseController.GamePaused = true;
        movement.MoveToPoint(endPosition, walkingSpeed);
        yield return new WaitForSeconds(.2f);
        StartCoroutine(uiManager.SmoothEndingGame());
    }
}
