using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackHitClose : MonoBehaviour
{
    [SerializeField] protected List<AudioClip> snd_attackHits;



    protected virtual void PlayerSFXAttack1Hit()
    {
        this.PlaySoundFX(this.snd_attackHits);
    }
    private void PlaySoundFX(List<AudioClip> sounds)
    {
        int random = Random.Range(0, sounds.Count);
        SoundFXManager.Instance.PlaySoundFXClip(sounds[random], transform, 1);
    }

    private void OnTriggerEnter(Collider other)
    {
        CharacterDamageReceiver damageReceiver = other.GetComponentInChildren<CharacterDamageReceiver>();
        if (damageReceiver != null)
        {
            this.PlayerSFXAttack1Hit();
        }
    }
}
