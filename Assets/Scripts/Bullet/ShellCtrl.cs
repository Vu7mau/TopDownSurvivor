using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellCtrl : MonoBehaviour
{
    [SerializeField] protected bool isPlayAudio = true;
    private void OnCollisionEnter(Collision collision)
    {
        if (isPlayAudio)
            SoundFXManager.Instance.PlaySoundFXClip(SoundFXManager.Instance.shellDrop, this.transform);
    }
}

