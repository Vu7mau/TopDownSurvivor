using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlaySMFX : MonoBehaviour
{
    public AudioClip StepSFX;
    public AudioClip beastAttackSFX;
    public AudioClip beastDeathSFX;
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
