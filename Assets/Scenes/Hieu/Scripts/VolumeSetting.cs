using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
public class VolumeSetting : MonoBehaviour
{
    [SerializeField] private AudioMixer MyMixer;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider SFX_Slider;
    private void Start()
    {
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
        MyMixer.SetFloat("BackGroundVolume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("_BackGroundVolume", volume);
    }
    public void SetSFXVolume()
    {
        float volume = SFX_Slider.value;
        MyMixer.SetFloat("SFX", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("SFX_Volume", volume);
    }
    private void LoadVolume()
    {
        musicSlider.value = PlayerPrefs.GetFloat("_BackGroundVolume");
        SFX_Slider.value = PlayerPrefs.GetFloat("SFX_Volume");
        SetMusicVolume();
        SetSFXVolume();
    }
}
