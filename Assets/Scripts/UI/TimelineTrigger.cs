using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TimelineTrigger : VuMonoBehaviour
{
    [SerializeField] private string timeLineName;
    [SerializeField] private bool isPlayOneTime = true;
    [SerializeField] private bool isPlayMusic = true;
    [SerializeField] private PlayableDirector timeLine;

    [SerializeField] private AudioClip audio;
    private bool played = false;
    protected override void LoadComponents()
    {
        this.LoadTimeLine();
    }

    private void LoadTimeLine()
    {
        if (!string.IsNullOrEmpty(timeLineName))
        {
            timeLine = GameObject.Find(timeLineName).GetComponent<PlayableDirector>();
            if (timeLine != null)
            {
                Debug.Log("LoadTimeLine success");
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && played == false)
        {
            if (timeLine == null) return;
            timeLine.gameObject.SetActive(true);
            timeLine.Play();
            if (isPlayOneTime)
                played = true;
            if (isPlayMusic)
                BackgroundMusicManager.Instance.PlayMusic(audio);
            Debug.Log("Play tutorial");
        }
    }
}
