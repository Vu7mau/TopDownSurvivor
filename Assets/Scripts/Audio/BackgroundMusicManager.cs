using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusicManager : Singleton<BackgroundMusicManager>
{
    

    [SerializeField] private AudioSource musicSource;

    [SerializeField] public AudioClip musicClip_1;

    [SerializeField] public AudioClip musicClip_2;
    [SerializeField] public AudioClip musicClip_3;

    protected override void Awake()
    {


    }
    protected override void LoadComponents()
    {
        if (musicSource == null)
        {
            musicSource = gameObject.AddComponent<AudioSource>();
            musicSource.playOnAwake = false;
        }
    }
    /// <summary>
    /// Phát một đoạn nhạc nền mới, thay thế clip hiện tại.
    /// </summary>
    /// <param name="clip">AudioClip cần phát</param>
    /// <param name="loop">Có lặp lại không</param>
    /// <param name="volume">Âm lượng từ 0 đến 1</param>
    public void PlayMusic(AudioClip clip, bool loop = true, float volume = 1f)
    {
        if (clip == null)
        {
            Debug.LogWarning("Clip is null, cannot play music.");
            return;
        }

        if (musicSource.isPlaying)
        {
            musicSource.Stop();
        }

        musicSource.clip = clip;
        musicSource.loop = loop;
        musicSource.volume = volume;
        musicSource.Play();
    }

    /// <summary>
    /// Tạm dừng nhạc.
    /// </summary>
    public void PauseMusic()
    {
        if (musicSource.isPlaying)
            musicSource.Pause();
    }

    /// <summary>
    /// Tiếp tục phát nhạc.
    /// </summary>
    public void ResumeMusic()
    {
        if (!musicSource.isPlaying && musicSource.clip != null)
            musicSource.Play();
    }

    /// <summary>
    /// Dừng hẳn nhạc và clear clip.
    /// </summary>
    public void StopMusic()
    {
        musicSource.Stop();
        musicSource.clip = null;
    }
}
