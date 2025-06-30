using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class AudioManagerTwo : MonoBehaviour
{
    [Header("Audio Source")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource sfxSource;
    [SerializeField] AudioSource buttonSource;
    [Header("Audio Clip")]
    [SerializeField] private AudioClip backgroundMusic;
    [SerializeField] private AudioClip button;
    [SerializeField] private AudioClip hover;
    [SerializeField] private AudioMixer mixer;
    [Header("Slider")]
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Slider buttonSlider;
    private void Start()
    {
        musicSource.clip = backgroundMusic;
        musicSource.Play();
        sfxSource.clip = button;
        buttonSource.clip = button;
    }
    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }
    public void MuteAll()
    {
        PlayerPrefs.SetFloat("MusicVolume", musicSlider.value);
        PlayerPrefs.SetFloat("SFXVolume", sfxSlider.value);
        PlayerPrefs.SetFloat("ButtonVolume", buttonSlider.value);

        mixer.SetFloat("Music", -80f);
        mixer.SetFloat("SFX", -80f);
        mixer.SetFloat("Button", -80f);

        musicSlider.SetValueWithoutNotify(0f);//Update Slider UI về 0 nhưng không gọi sự kiện
        sfxSlider.SetValueWithoutNotify(0f);
        buttonSlider.SetValueWithoutNotify(0f);
    }
    public void UnmuteAll()
    {
        float v;
        v = PlayerPrefs.GetFloat("MusicVolume", 1f);
        mixer.SetFloat("Music", Mathf.Log10(Mathf.Max(v, 0.0001f)) * 20);
        musicSlider.SetValueWithoutNotify(v);   

        v = PlayerPrefs.GetFloat("SFXVolume", 1f);
        mixer.SetFloat("SFX", Mathf.Log10(Mathf.Max(v, 0.0001f)) * 20);
        sfxSlider.SetValueWithoutNotify(v);

        v = PlayerPrefs.GetFloat("ButtonVolume", 1f);
        mixer.SetFloat("Button", Mathf.Log10(Mathf.Max(v, 0.0001f)) * 20);
        buttonSlider.SetValueWithoutNotify(v);
    }
}
