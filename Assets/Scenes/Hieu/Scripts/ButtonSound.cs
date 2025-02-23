using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSound : MonoBehaviour
{
    public AudioClip ClickSound;
    public AudioSource audioSource;
    private void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.clip = ClickSound;
        Button[] buttons = FindObjectsOfType<Button>();
        foreach (Button b in buttons)
        {
            b.onClick.AddListener(() => PlayClickSound());
        }
        void PlayClickSound()
        {
            if (audioSource && ClickSound)
            {
                audioSource.PlayOneShot(ClickSound);
            }
        }
    }
}
