using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectFXSound : MonoBehaviour
{
    [SerializeField] private AudioClip snd_effect;
    AudioSource t;
    private void Awake()
    {
        t = gameObject.GetComponent<AudioSource>();
    }
    private void OnEnable()
    {
        
        t.clip = snd_effect;
        t.playOnAwake = false;
        t.loop = false;
        t.Play();
        t.volume = 5f;
    }
    private void OnDisable()
    {
        t.clip = snd_effect;
        t.playOnAwake = false;
        t.loop = false;
        t.Stop();
        t.volume = 0f;
    }
}
