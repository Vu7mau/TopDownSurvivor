using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEnemyManager : Singleton<SoundEnemyManager>
{
    [SerializeField] protected Transform holder;
    [SerializeField] protected List<Transform> poolSoundFX;
    [SerializeField] private AudioSource soundFXObject;
    [SerializeField] private AudioSource backgroundMusic;


    protected override void LoadComponents()
    {

        this.LoadHolder();
    }
    public virtual void PlayEnemySoundFXClip(AudioClip clip, Transform transformSpawn, bool isMusic, float volume)
    {
        AudioSource objSpawn = (isMusic) ? backgroundMusic : soundFXObject;
        AudioSource audi = GetObjectFromPool(objSpawn, transformSpawn);

        audi.clip = clip;

        audi.volume = volume;

        audi.Play();

        float _clipLenght = audi.clip.length;
        // Destroy(audi.gameObject, clipLenght);
        if(!isMusic) StartCoroutine(Despawn(audi.transform, _clipLenght));
    }

    public virtual void PlayEnemySoundFXClip(AudioClip clip, Transform transformSpawn, float volume = 1)
    {
        this.PlayEnemySoundFXClip(clip, transformSpawn, false, volume);
    }

    public virtual void StopEnemySoundFXClip(AudioClip clip, bool isMusic)
    {
        AudioSource objSpawn = (isMusic) ? backgroundMusic : soundFXObject;
        StartCoroutine(Despawn(objSpawn.transform, 0f));
    }


    protected void LoadHolder()
    {
        if (this.holder != null) return;


        this.holder = transform.Find("Holder");
        //Debug.Log(holder.transform.name + " Load HolderSoundFX ");
    }
    protected AudioSource GetObjectFromPool(AudioSource sound, Transform transformSpawn)
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

        AudioSource audioSource1 = Instantiate(sound, transformSpawn);
        audioSource1.transform.parent = this.holder;
        audioSource1.name = sound.gameObject.name;
        return audioSource1;
    }
    protected virtual IEnumerator Despawn(Transform obj, float duration)
    {
        yield return new WaitForSeconds(duration);
        this.poolSoundFX.Add(obj);
        obj.gameObject.SetActive(false);
    }
}
