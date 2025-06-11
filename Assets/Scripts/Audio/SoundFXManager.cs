using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundFXManager : AudioManager
{
    public static SoundFXManager Instance { get; set; }
    [Space]
    [Header("SoundFXManager")]
    [Space]
    [Header("Weapon Shooter")]
    [SerializeField] public  AudioClip rifleShoot;
    [SerializeField] public  AudioClip pistloShoot;
    [SerializeField] public  AudioClip flamethrowerShoot;
    [SerializeField] public  AudioClip machineShoot;
    [SerializeField] public  AudioClip shotgunShoot;
    [SerializeField] public  AudioClip lazerShoot;
    [Space]
    [Header("Weapon Equip")]
    [SerializeField] public AudioClip machineEquip;
    [SerializeField] public AudioClip shotgunEquip;
    [Space]
    [Header("Weapon Reload Ammour")]
    [SerializeField] public  AudioClip reloadRiffle;
    [SerializeField] public AudioClip machineReload;
    [Space]
    [Header("Weapon Reload Explosion")]
    [SerializeField] public  AudioClip rocketExplosion;
    [SerializeField] public  AudioClip lazerExplosion;
    [Space]
    [Header("Character")]
    [SerializeField] public  AudioClip maleHit;
    [SerializeField] public  AudioClip[] footStep;

    [Space]
    [Header("Other")]
    [SerializeField] public  AudioClip pickUp;
    [SerializeField] public  AudioClip leveUp;
    [SerializeField] public  AudioClip shellDrop;
    [SerializeField] public  AudioClip handlingGun;
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
