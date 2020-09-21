using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class OptionsMenu : MonoBehaviour
{
    
    public AudioMixer masterMixer;


    public void SetMasterVolume (float sliderValue)
    {
        masterMixer.SetFloat("MasterVolume", Mathf.Log10(sliderValue) * 20);
    }

    public void SetMusicVolume(float sliderValue)
    {
        masterMixer.SetFloat("MusicVolume", Mathf.Log10(sliderValue) * 20);
    }

    public void SetSoundVolume(float sliderValue)
    {
        masterMixer.SetFloat("SoundVolume", Mathf.Log10(sliderValue) * 20);
    }

    public void SetAmbienceVolume(float sliderValue)
    {
        masterMixer.SetFloat("AmbienceVolume", Mathf.Log10(sliderValue) * 20);
    }

    public void SetFullscreen(bool is_Fullscreen)
    {
        Screen.fullScreen = is_Fullscreen;
        Debug.Log("Fullscreen is " + is_Fullscreen);
    }
}
