using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class AudioManage : MonoBehaviour
{
    [SerializeField] private AudioSource music_source;
    [SerializeField] private AudioSource SFX_Source;
    public AudioClip BackGround;
    public AudioClip Click;
    public static AudioManage Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }   
    public void Start()
    {
        music_source.clip = BackGround;
        music_source.Play();
    }
    public void PlaySFX(AudioClip clip)
    {        
        SFX_Source.PlayOneShot(clip);
    }
}
