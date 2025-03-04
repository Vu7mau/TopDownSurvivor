using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSound : MonoBehaviour
{
    public AudioClip ClickSound;
    public AudioClip clickSound2;
    public AudioClip clickSoundSkin;
    public AudioClip clickSoundSkilltree;
    public AudioSource audioSource;    
    [SerializeField] private Button[] buttons;
    [SerializeField] private Button[] buttons2;
    [SerializeField] private Button[] buttonsSkin;
    [SerializeField] private GameObject ObjSkillHolder;
    private Button[] buton;
    private void Start()
    {
        buton = ObjSkillHolder.GetComponentsInChildren<Button>();
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.clip = ClickSound;        
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
        foreach (Button button in buttons2)
        {
            button.onClick.AddListener(()=>playClickSound2());
        }
        void playClickSound2()
        {
            if (audioSource && clickSound2)
            {
                audioSource.PlayOneShot(clickSound2);
            }
        }
        foreach (Button button in buttonsSkin)
        {
            button.onClick.AddListener(() => playClickSound3());
        }
        void playClickSound3()
        {
            if (audioSource && clickSound2)
            {
                audioSource.PlayOneShot(clickSoundSkin);
            }
        }
        foreach (Button button in buton)
        {
            button.onClick.AddListener(() => playClickSound4());
        }
        void playClickSound4()
        {        
            if (audioSource && clickSound2)
            {
                audioSource.PlayOneShot(clickSoundSkilltree);
            }
        }
    }
}
