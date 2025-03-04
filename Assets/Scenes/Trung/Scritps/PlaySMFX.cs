using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlaySMFX : MonoBehaviour
{

    private static PlaySMFX instance;
    public static PlaySMFX Instance { get { return instance; } }
    public AudioClip StepSFX;
    public AudioClip beastAttackSFX;
    public AudioClip beastHurtSFX;
    public AudioClip beastDeathSFX;

    private void Awake()
    {
        instance = this;
    }
    public void ChaseSFX()
    {
        SoundFXManager.Instance.PlaySoundFXClip(StepSFX, transform);
    }
    public void AttackSFX()
    {
        SoundFXManager.Instance.PlaySoundFXClip(beastAttackSFX, transform);
    }
    public void DeathSFX()
    {
        SoundFXManager.Instance.PlaySoundFXClip(beastDeathSFX, transform);
    }
}
