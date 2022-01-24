using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MouseManager : MonoBehaviour
{
    private static readonly string FirstPlay = "FirstPlay";
    private static readonly string MouseSensivityPref = "MouseSensivityPref";
    private static readonly string MouseSizePref = "MouseSizePref";
    private int firstPlayInt;

    [SerializeField] Slider mouseSensivitySlider, mouseSizeSlider;
    private float mouseSensivity, mouseSize;

    // Start is called before the first frame update
    void Start()
    {
        if (firstPlayInt == 0)
        {
            mouseSensivity = 0.5f;
            mouseSize = 0.7f;
            mouseSensivitySlider.value = mouseSensivity;
            mouseSizeSlider.value = mouseSize;
            PlayerPrefs.SetFloat(MouseSensivityPref, mouseSensivity);
            PlayerPrefs.SetFloat(MouseSizePref, mouseSize);
            PlayerPrefs.SetInt(FirstPlay, 1);

        }
        else
        {
            mouseSensivity = PlayerPrefs.GetFloat(MouseSensivityPref);
            mouseSensivitySlider.value = mouseSensivity;
            mouseSize = PlayerPrefs.GetFloat(MouseSizePref);
            mouseSensivitySlider.value = mouseSize;
        }
        Screen.fullScreen = true;
    }

    public void SaveMouseSettings()
    {
        PlayerPrefs.SetFloat(MouseSensivityPref, mouseSensivitySlider.value);
        PlayerPrefs.SetFloat(MouseSizePref, mouseSizeSlider.value);
    }

    
    private void OnApplicationFocus(bool focus)
    {
        if (!focus)
        {
            SaveMouseSettings();
        }
    }

    public void SetFullScreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }
}
