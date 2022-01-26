using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    private static readonly string FirstPlay = "FirstPlay";
    private static readonly string MusicPref = "MusicPref";
    private static readonly string SoundEffectsPref = "SoundEffectsPref";
    private int firstPlayInt;


    [SerializeField] Slider musicSlider, soundEffectsSlider;
    private float musicVolume, soundEffectsVolume;

    [SerializeField] AudioSource musicAudio;
    [SerializeField] AudioSource[] soundEffectsAudio;

    void Start()
    {
        firstPlayInt = PlayerPrefs.GetInt(FirstPlay);

        if (firstPlayInt == 0)
        {
            musicVolume = 0.5f;
            soundEffectsVolume = 0.7f;
            musicSlider.value = musicVolume;
            soundEffectsSlider.value = soundEffectsVolume;
            PlayerPrefs.SetFloat(MusicPref, musicVolume);
            PlayerPrefs.SetFloat(SoundEffectsPref, soundEffectsVolume);
            PlayerPrefs.SetInt(FirstPlay, 1);
            
        }
        else
        {
            if (PlayerPrefs.GetFloat(MusicPref) == 0)
            {
                musicSlider.value = 0.00001f;
            }
            if (PlayerPrefs.GetFloat(SoundEffectsPref) == 0)
            {
                soundEffectsSlider.value = 0.00001f;
            }
            musicVolume = PlayerPrefs.GetFloat(MusicPref);
            musicSlider.value = musicVolume;
            soundEffectsVolume = PlayerPrefs.GetFloat(SoundEffectsPref);
            soundEffectsSlider.value = soundEffectsVolume; 
        }
    }


    public void SaveSoundSettings()
    {
        PlayerPrefs.SetFloat(MusicPref, musicSlider.value);
        PlayerPrefs.SetFloat(SoundEffectsPref, soundEffectsSlider.value);
    }


    private void OnApplicationFocus(bool focus)
    {
        if (!focus)
        {
            SaveSoundSettings();
        }
    }

    public void UpdateSound()
    {
        musicAudio.volume = musicSlider.value;
        for (int i = 0; i < soundEffectsAudio.Length; i++)
        {
            soundEffectsAudio[i].volume = soundEffectsSlider.value;
        }
    }
    

}
