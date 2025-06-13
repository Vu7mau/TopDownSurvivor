using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class VolumeSettings : MonoBehaviour
{
    [SerializeField] private AudioMixer myMixer;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Slider buttonSlider;
    private void Start()
    {
        if (PlayerPrefs.HasKey("MusicVolume")) LoadVolume();
        else SetMusicVolume();
        SetSFXVolume();
        SetButtonVolume();
    }
    public void SetMusicVolume()
    {
        float volume = musicSlider.value;
        myMixer.SetFloat("Music", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("MusicVolume", volume);
    }
    public void SetSFXVolume()
    {
        float volume = sfxSlider.value;
        myMixer.SetFloat("SFX", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("SFXVolume", volume);
    }
    public void SetButtonVolume()
    {
        float volume = buttonSlider.value;
        myMixer.SetFloat("Button", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("ButtonVolume", volume);
    }
    private void LoadVolume()
    {
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume");
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume");
        buttonSlider.value = PlayerPrefs.GetFloat("ButtonVolume");
        SetMusicVolume();
        SetSFXVolume();
        SetButtonVolume();
    }
}
