using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectFXSound : MonoBehaviour
{
    [SerializeField] private List<AudioClip> snd_Effect;
    //AudioSource t;
    private void Awake()
    {
        //t = gameObject.GetComponent<AudioSource>();
    }
    private void OnEnable()
    {
        int random = Random.Range(0, snd_Effect.Count);
        if (this.snd_Effect.Count == 0) return;
        //t.clip = snd_Explosion_Effect[random];
        //t.playOnAwake = false;
        //t.loop = false;
        //t.Play();
        //t.volume = 5f;
        SoundFXManager.Instance.PlaySoundFXClip(this.snd_Effect[random],transform,1);
    }
    //private void OnDisable()
    //{
    //    int random = Random.Range(0, snd_Explosion_Effect.Count);
    //    t.clip = snd_Explosion_Effect[random];
    //    t.playOnAwake = false;
    //    t.loop = false;
    //    t.Stop();
    //    t.volume = 0f;
    //}
}
