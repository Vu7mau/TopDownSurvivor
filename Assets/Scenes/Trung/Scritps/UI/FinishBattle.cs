using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishBattle : MonoBehaviour
{
    [SerializeField] private AudioClip snd_finish;
    private void Start()
    {
        SoundFXManager.Instance.PlaySoundFXClip(snd_finish,transform);
    }
}
