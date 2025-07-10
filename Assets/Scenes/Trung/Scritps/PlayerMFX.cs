using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMFX : MonoBehaviour
{
    [SerializeField] protected AudioClip bg;
    protected AudioSource musicBg;
    protected virtual void Awake()
    {
        musicBg = GetComponent<AudioSource>();
    }
    protected virtual void Start()
    {
        musicBg.clip = bg;
        musicBg.Play();
    }
}
