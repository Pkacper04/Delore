using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    private static readonly string MusicPref = "MusicPref";
    private static readonly string SoundEffectsPref = "SoundEffectsPref";

    public AudioMixer SFXMixer;

    [SerializeField] Slider musicSlider, soundEffectsSlider;

    private void Start()
    {
        if(PlayerPrefs.HasKey(MusicPref))
        {
            musicSlider.value = PlayerPrefs.GetFloat(MusicPref);
            soundEffectsSlider.value = PlayerPrefs.GetFloat(SoundEffectsPref);
            SFXMixer.SetFloat("musicVolume", musicSlider.value);
            SFXMixer.SetFloat("sfxVolume", soundEffectsSlider.value);
        }
        else
        {
            musicSlider.value = 0;
            soundEffectsSlider.value = 0;
        }
    }

    public void UpdateSFXSound(Slider slider)
    {
        SFXMixer.SetFloat("sfxVolume", slider.value);
    }

    public void UpdateMusicSound(Slider slider)
    {
        SFXMixer.SetFloat("musicVolume", slider.value);
    }

    public void SaveVolume()
    {
        PlayerPrefs.SetFloat(MusicPref, musicSlider.value);
        PlayerPrefs.SetFloat(SoundEffectsPref, soundEffectsSlider.value);
    }

    public void OntoggleChange(Toggle toggle)
    {
        Screen.fullScreen = toggle.isOn;
    }
}
