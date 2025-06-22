using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellCtrl : MonoBehaviour
{
    [SerializeField] protected bool isPlayAudio = true;
    [SerializeField] protected bool isPlayed = false;

    private void OnEnable()
    {
        isPlayed=false;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (isPlayAudio&&!isPlayed)
        {
            SoundFXManager.Instance.PlaySoundFXClip(SoundFXManager.Instance.shellDrop, this.transform);
            isPlayed=true;
        }
    }
}

