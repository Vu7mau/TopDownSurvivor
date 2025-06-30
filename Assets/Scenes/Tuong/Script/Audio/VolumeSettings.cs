using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
public class VolumeSettings : MonoBehaviour
{
    [SerializeField] private AudioMixer myMixer;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Slider buttonSlider;

    //[SerializeField] private Button muteOnButton;
    //[SerializeField] private Button muteOffButton;

    private bool isMuted = false;
    private void Start()
    {
        isMuted = PlayerPrefs.GetInt("IsMuted", 0) == 1;
        if (PlayerPrefs.HasKey("MusicVolume")) LoadVolume();
        else
        {
            FirstTimeSetup();
        }
        //ApplyMuteState();
        //UpdateMuteButtons();
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
    public void OnMuteButtonPressed()
    {
        isMuted = true;
        PlayerPrefs.SetInt("IsMuted", 1);
        PlayerPrefs.Save();
        ApplyMuteState();
        //UpdateMuteButtons();
    }
    public void OnUnMuteButtonPressed()
    {
        isMuted = false;
        PlayerPrefs.SetInt("IsMuted", 0);
        PlayerPrefs.Save();
        ApplyMuteState();
        //UpdateMuteButtons();
    }
    public void ApplyMuteState()
    {
        if (isMuted)
        {
            myMixer.SetFloat("Music", -80f);
            myMixer.SetFloat("SFX", -80f);
            myMixer.SetFloat("Button", -80f);
        }
        else
        {
            ApplyAllVolumes();
        }
    }
    //private void UpdateMuteButtons()
    //{
    //    muteOnButton.gameObject.SetActive(!isMuted);
    //    muteOffButton.gameObject.SetActive(isMuted);
    //}
}
