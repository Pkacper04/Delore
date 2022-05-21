using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ButtonEventHandler : MonoBehaviour
{
    [SerializeField] private AudioSource source;
    [SerializeField] private AudioClip highlightClip;
    [SerializeField] private AudioClip clikedClip;

    public void PlayHighlightSound()
    {
        source.clip = highlightClip;
        source.loop = true;
        source.Play();
    }

    public void StopHighlightSound()
    {
        if (source.clip == clikedClip && source.isPlaying)
            return;
        source.clip = null;
        source.loop = false;
        source.Stop();
    }

    public void clickedButton()
    {
        source.loop = false;
        source.Stop();
        source.clip = clikedClip;
        source.Play();
    }
}
