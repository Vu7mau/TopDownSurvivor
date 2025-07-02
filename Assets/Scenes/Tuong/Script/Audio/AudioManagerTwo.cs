using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
public class AudioManagerTwo : MonoBehaviour
{
    public static AudioManagerTwo Instance;
    [System.Serializable]
    public struct ButtonSFXEntry
    {
        public ButtonSFXType type;
        public AudioClip clip;
    }
    [Header("Audio Source")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource sfxSource;
    [SerializeField] AudioSource buttonSource;
    [Header("Audio Clip")]
    [SerializeField] private AudioClip backgroundMusic;
    [SerializeField] private AudioClip button;
    [SerializeField] private List<ButtonSFXEntry> buttonSFXList;
    private Dictionary<ButtonSFXType, AudioClip> buttonSFXDict;
    private void Start()
    {
        if (Instance == null) Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
        musicSource.clip = backgroundMusic;
        musicSource.loop = true;
        musicSource.Play();
        sfxSource.clip = button;
        buttonSource.clip = button;
        buttonSFXDict = new Dictionary<ButtonSFXType, AudioClip>();
        foreach (var entry in buttonSFXList)
        {
            if (!buttonSFXDict.ContainsKey(entry.type))
            {
                buttonSFXDict.Add(entry.type, entry.clip);
            }
        }
    }
    public void PlaySFX(AudioClip clip)
    {
        if(sfxSource!= null) 
            sfxSource.PlayOneShot(clip);
    }
    public void PlayButtonSFX(ButtonSFXType type)
    {
        if (buttonSFXDict.TryGetValue(type, out var clip) && clip != null)
        {
            buttonSource.PlayOneShot(clip);
        }
    }
}
