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
        else
        {
            FirstTimeSetup();
        }
        ApplyAllVolumes();

    }
    private void FirstTimeSetup()
    {
        PlayerPrefs.SetFloat("MusicVolume", musicSlider.value);
        PlayerPrefs.SetFloat("SFXVolume", sfxSlider.value);
        PlayerPrefs.SetFloat("ButtonVolume", buttonSlider.value);
        ApplyAllVolumes();
    }
    public void SetMusicVolume()
    {
        float volume = Mathf.Max(musicSlider.value, 0.0001f);
        myMixer.SetFloat("Music", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("MusicVolume", volume);
    }
    public void SetSFXVolume()
    {
        float volume = Mathf.Max(sfxSlider.value, 0.0001f);
        myMixer.SetFloat("SFX", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("SFXVolume", volume);
    }
    public void SetButtonVolume()
    {
        float volume = Mathf.Max(buttonSlider.value, 0.0001f);
        myMixer.SetFloat("Button", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("ButtonVolume", volume);
    }
    private void ApplyAllVolumes()
    {
        SetMusicVolume();
        SetSFXVolume();
        SetButtonVolume();
    }
    private void LoadVolume()
    {
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume");
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume");
        buttonSlider.value = PlayerPrefs.GetFloat("ButtonVolume");
        ApplyAllVolumes();
    }
    
}
