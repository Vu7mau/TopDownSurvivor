using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSetting : MonoBehaviour
{
    [SerializeField] private AudioMixer MyMixer;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider SFX_Slider;
    AudioManager _audioManager;
    
    private void Start()
    {
        _audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
        if (PlayerPrefs.HasKey("_BackGroundVolume"))
        {
            LoadVolume();
        }
        else
        {
            SetMusicVolume();
            SetSFXVolume();
        }
    }
    public void SetMusicVolume()
    {
        float volume = musicSlider.value;
        MyMixer.SetFloat("BackGroundVolume", Mathf.Log10(volume)*20);
        PlayerPrefs.SetFloat("_BackGroundVolume",volume);
    }
    public void SetSFXVolume()
    {
        float volume = SFX_Slider.value;
        MyMixer.SetFloat("SFX", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("SFX_Volume",volume );
    }
    private void LoadVolume()
    {
        musicSlider.value = PlayerPrefs.GetFloat("_BackGroundVolume");
        SFX_Slider.value = PlayerPrefs.GetFloat("SFX_Volume");
        SetMusicVolume();
        SetSFXVolume();
    }
    public void musicClick()
    {
        _audioManager.PlaySFX(_audioManager.Click);
    }
}
