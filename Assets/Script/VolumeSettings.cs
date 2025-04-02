using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider SFXSlider;

    void Start()
    {
        if (PlayerPrefs.HasKey("MusicVolume"))
        {
            LoadMusicVolume();
        }
        else
        {
            SetMusicVolume();
            SetSFXVolume();
        }
        SetMusicVolume();
    }
    public void SetMusicVolume()
    {
        float volume = musicSlider.value;
        audioMixer.SetFloat("Music", MathF.Log10(volume) * 20);
        PlayerPrefs.SetFloat("MusicVolume", volume);
    }

    public void SetSFXVolume()
    {
        float volume = SFXSlider.value;
        audioMixer.SetFloat("SFX", MathF.Log10(volume) * 20);
        PlayerPrefs.SetFloat("SFXVolume", volume);
    }

    private void LoadMusicVolume()
    {
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume");
        SFXSlider.value = PlayerPrefs.GetFloat("SFXVolume");
        SetMusicVolume();
        SetSFXVolume();
    }
}
