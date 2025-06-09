using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySound : MonoBehaviour
{
    public AudioClip audioClip;
    public AudioSource audioClip2D;

    public void PlayAudioOneShot()
    {
        audioClip2D.PlayOneShot(audioClip);
    }
}
