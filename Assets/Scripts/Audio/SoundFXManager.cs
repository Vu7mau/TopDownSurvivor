using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundFXManager : AudioManager
{
    public static SoundFXManager Instance { get; set; }
    [Space]
    [Header("SoundFXManager")]
    [Space]
    [Header("Weapon")]
    [SerializeField] public  AudioClip rifleShoot;
    [SerializeField] public  AudioClip pistloShoot;
    [SerializeField] public  AudioClip rocketExplosion;
    [SerializeField] public  AudioClip reloadAmmor;
    [Space]
    [Header("Other")]
    [SerializeField] public  AudioClip leveUp;
    [SerializeField] public  AudioClip pickUp;
    [SerializeField] public  AudioClip maleHit;
    [SerializeField] public  AudioSource bgMusic;
  

    protected override void Awake()
    {
        base.Awake();
        if (Instance == null)
            Instance = this;
    }
    protected override void Start()
    {
        base.Start();
        bgMusic.Play();
    }
}
