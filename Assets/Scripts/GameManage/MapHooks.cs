// File: MapHooks.cs
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;

public class MapHooks : MonoBehaviour
{
    [Header("Events")]
    public UnityEvent OnEnter;
    public UnityEvent OnExit;

    [Header("Optional Helpers")]
    [SerializeField] private Light lightToFade;
    [SerializeField] private float fadeDuration = 2f;

    [SerializeField] private PlayableDirector timeline;
    [SerializeField] private float skipTime = 3f;

    [SerializeField] private bool turnOnCharacterLightOnEnter = true;

    public void InvokeEnter()
    {
    
        if (turnOnCharacterLightOnEnter && CharacterCtrl.Instance && CharacterCtrl.Instance.CharacterEffect)
            CharacterCtrl.Instance.CharacterEffect.TurnOnLight();
        if (lightToFade) StartCoroutine(FadeOutLightCoroutine(fadeDuration));
        if (timeline) timeline.Play();

        OnEnter?.Invoke();
    }

    public void InvokeExit()
    {
       
        OnExit?.Invoke();
    }

    public void SkipTimelineForward()
    {
        if (!timeline || timeline.state != PlayState.Playing) return;
        float newTime = Mathf.Clamp((float)timeline.time + skipTime, 0f, (float)timeline.duration - 0.01f);
        timeline.time = newTime;
        timeline.Evaluate();
    }

    private IEnumerator FadeOutLightCoroutine(float duration)
    {
        float start = lightToFade.intensity;
        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            float k = t / duration;
            lightToFade.intensity = Mathf.Lerp(start, 0f, k);
            yield return null;
        }
        lightToFade.intensity = 0f;
    }
}
