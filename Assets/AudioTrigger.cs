using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class AudioTrigger : MonoBehaviour
{
    [SerializeField]
    private AudioSource source;

    [SerializeField]
    private bool haveAmbient = false;

    [SerializeField, ShowIf("haveAmbient")]
    private AudioSource ambientSound;

    public void playOneTime(AudioClip clip)
    {
        source.loop = false;
        source.clip = clip;
        source.Play();
    }

    public void PlayLoop(AudioClip clip)
    {
        source.loop = true;
        source.clip = clip;
        source.Play();
    }

    public void PlayOneAmbient(AudioClip clip)
    {
        ambientSound.clip = clip;
        ambientSound.loop = false;
        ambientSound.Play();
    }

    public void PlayAmbientLoop(AudioClip clip)
    {
        ambientSound.clip = clip;
        ambientSound.loop = true;
        ambientSound.Play();
    }

    public void StopAmbient()
    {
        ambientSound.Stop();
    }

    public void PlayAmbient()
    {
        ambientSound.Play();
    }

    public void StopCurrentClip()
    {
        source.Stop();
    }

    public void PlayCurrentClip()
    {
        source.Play();
    }


}
