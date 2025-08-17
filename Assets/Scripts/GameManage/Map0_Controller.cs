// File: Map0_Controller.cs
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

public class Map0_Controller : Map_Controller
{
    [SerializeField] private Light light;
    [SerializeField] private PlayableDirector tutorial_1;
    [SerializeField] private float skipTime = 2f;

    public static Action CalledTutorial;

    private void OnTriggerEnter(Collider other)
    {
        //if (other.CompareTag("Player"))
        //{
        //    gameController.SwitchMap(mapIndex);
        //    CharacterCtrl.Instance.CharacterEffect.TurnOnLight();
        //    FadeOutLight(2f);
        //}
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Q)) SkipForward(skipTime);
    }

    public void SkipForward(float delta)
    {
        if (!tutorial_1 || tutorial_1.state != PlayState.Playing) return;
        float newTime = Mathf.Clamp((float)tutorial_1.time + delta, 0f, (float)tutorial_1.duration - 0.01f);
        tutorial_1.time = newTime;
        tutorial_1.Evaluate();
    }

    private void OnCutsceneIn(PlayableDirector pd)
    {
        gameController.MoveCharacterPos(currentMapSpawnPoint);
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        CalledTutorial += TutorialIntro;
        // nếu cần: tutorial_1.played += OnCutsceneIn;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        CalledTutorial -= TutorialIntro;
        // nếu cần: tutorial_1.played -= OnCutsceneIn;
    }

    private void TutorialIntro() => tutorial_1?.Play();

    public void FadeOutLight(float duration) => StartCoroutine(FadeOutCoroutine(duration));

    private IEnumerator FadeOutCoroutine(float duration)
    {
        if (!light) yield break;
        float startIntensity = light.intensity;
        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            light.intensity = Mathf.Lerp(startIntensity, 0f, t / duration);
            yield return null;
        }
        light.intensity = 0f;
    }
}
