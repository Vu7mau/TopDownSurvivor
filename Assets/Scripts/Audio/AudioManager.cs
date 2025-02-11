using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  class AudioManager : VuMonoBehaviour
{
   // public static SoundFXManager Instance {  get; set; }



    [SerializeField] protected Transform holder;
    [SerializeField] protected List<Transform> poolSoundFX;
    [SerializeField] private AudioSource soundFXObject;
    [SerializeField] private AudioSource backgroundMusic;
    

    protected override void LoadComponents()
    {
             
        this.LoadHolder();
    }
    public virtual void PlaySoundFXClip(AudioClip clip, Transform transformSpawn, bool isMusic)
    {
        AudioSource objSpawn = (isMusic) ? backgroundMusic : soundFXObject;
        AudioSource audi = GetObjectFromPool(objSpawn, transformSpawn);

        audi.clip = clip;

     //   audi.volume = 1f;

        audi.Play();

        float clipLenght = audi.clip.length;
        // Destroy(audi.gameObject, clipLenght);
        StartCoroutine(Despawn(audi.transform, clipLenght));

    }
    public virtual void PlaySoundFXClip(AudioClip clip, Transform transformSpawn/*, float volume*/)
    {
        this.PlaySoundFXClip(clip, transformSpawn,false);
    }



    protected  void LoadHolder()
    {
        if (this.holder != null) return;

     
        this.holder = transform.Find("Holder");
        //Debug.Log(holder.transform.name + " Load HolderSoundFX ");
    }
    protected  AudioSource GetObjectFromPool(AudioSource sound, Transform transformSpawn)
    {
        foreach (Transform obj in poolSoundFX)
        {
            AudioSource audioSource = obj.GetComponent<AudioSource>();
            if (audioSource != null && obj.gameObject.name == soundFXObject.gameObject.name)
            {
                this.poolSoundFX.Remove(obj.transform);
                audioSource.gameObject.SetActive(true);
                return audioSource;
            }
        }

        AudioSource audioSource1 = Instantiate(sound,transformSpawn);
        audioSource1.transform.parent = this.holder;
        audioSource1.name= sound.gameObject.name;
        return audioSource1;
    }
    protected virtual IEnumerator Despawn(Transform obj, float duration)
    {
        yield return new WaitForSeconds(duration);
        this.poolSoundFX.Add(obj);
        obj.gameObject.SetActive(false);
    }
}
