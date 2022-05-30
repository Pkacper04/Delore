using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Delore.Player;
using Delore.AI;

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

    [SerializeField]
    private AudioSource[] SoundsToDissable;

    [SerializeField]
    private AudioSource[] enemiesSounds;

    [SerializeField]
    private AIMovement[] enemies;

    [SerializeField]
    private AudioSource sourcePingPong;

    [SerializeField]
    private AudioClip endMusic;

    [SerializeField]
    private float fadeDuration;

    public void GameEnd()
    {
        foreach(AIMovement enemy in enemies)
        {
            enemy.endGame = true;
        }
        StartCoroutine(SoundFading.FadeInCoroutine(enemiesSounds,fadeDuration,1,0));
        StartCoroutine(SoundFading.FadeInCoroutine(SoundsToDissable,fadeDuration,1,0));
        StartCoroutine(SoundFading.FadePingPong(sourcePingPong, endMusic, fadeDuration, 1, 0));
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
