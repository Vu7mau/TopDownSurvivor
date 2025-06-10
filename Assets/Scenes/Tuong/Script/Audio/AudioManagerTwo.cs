using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}
