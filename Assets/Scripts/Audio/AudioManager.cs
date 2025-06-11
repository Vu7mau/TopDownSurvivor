using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : VuMonoBehaviour
{

    [SerializeField] protected Transform holder;
    [SerializeField] protected List<Transform> poolSoundFX;
    [SerializeField] private AudioSource soundFXObject;
    [SerializeField] private AudioSource backgroundMusic;


    protected override void LoadComponents()
    {

        this.LoadHolder();
    }
    public virtual void PlaySoundFXClip(AudioClip clip, Transform transformSpawn, bool isMusic, float clipLength = 0, float startTime = 0)
    {
        AudioSource objSpawn = (isMusic) ? backgroundMusic : soundFXObject;
        AudioSource audi = GetObjectFromPool(objSpawn, transformSpawn);

        audi.clip = clip;

        //   audi.volume = 1f;
        if (startTime > 0)
        {
            audi.time = startTime;
            StartCoroutine(FadeIn(audi,.5f));
        }
        else
            audi.Play();
        float _clipLenght = 0;
        if (clipLength > 0)
            _clipLenght = clipLength;
        else
            _clipLenght = audi.clip.length;
        // Destroy(audi.gameObject, clipLenght);
        StartCoroutine(Despawn(audi.transform, _clipLenght));
    }
    IEnumerator FadeIn(AudioSource audioSource, float fadeDuration)
    {
        audioSource.volume = 0f;
        audioSource.Play();
        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(0f, 1f, timer / fadeDuration);
            yield return null;
        }

        audioSource.volume = 1f;
    }
    public virtual void PlaySoundFXClip(AudioClip clip, Transform transformSpawn/*, float volume*/)
    {
        this.PlaySoundFXClip(clip, transformSpawn, false);
    }
    public virtual void PlaySoundFXClip(AudioClip clip, Transform transformSpawn, float clipLength, float startTime)
    {
        this.PlaySoundFXClip(clip, transformSpawn, false, clipLength, startTime);
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
