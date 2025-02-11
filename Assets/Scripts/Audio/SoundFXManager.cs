using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundFXManager : AudioManager
{
    public static SoundFXManager Instance { get; set; }
    [Space]
    [Header("SoundFXManager")]
    [SerializeField] public  AudioClip rifleShoot;

    protected override void Awake()
    {
        base.Awake();
        if (Instance == null)
            Instance = this;
    }
}
