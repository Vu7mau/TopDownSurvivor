using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Localization.Plugins.XLIFF.V20;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Rendering;

public class Map0_Controller : Map_Controller
{
    [SerializeField] private Light light;
    [SerializeField] PlayableDirector tutorial_1;
    [SerializeField] float skipTime;

    public static Action CalledTutorial;
    
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameController.SwitchMap(mapIndex);
            this.processing.gameObject.SetActive(false);
            CharacterCtrl.Instance.CharacterEffect.TurnOnLight();
            FadeOutLight(2);
        }
    }
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.S))
        {
            SkipForward(skipTime);
        }
    }
    public void SkipForward(float skipTime)
    {
        if (tutorial_1 == null || tutorial_1.state != PlayState.Playing)
            return;

        float currentTime = (float)tutorial_1.time;
        float newTime = currentTime + skipTime;

        // Clamp để không vượt quá duration
        newTime = Mathf.Clamp(newTime, 0f, (float)tutorial_1.duration - 0.01f);

        tutorial_1.time = newTime;
        tutorial_1.Evaluate();
    }
    private void OnCutsceneIn(PlayableDirector pd)
    {
        gameController.MoveCharacterPos(currentMapSpawnPoint);
    }    
    protected override void OnEnable()
    {
        if (map.gameObject.activeSelf)
        {
            CharacterCtrl.Instance.CharacterEffect.TurnOffLight();
        }
        CalledTutorial += TutorialIntro;


    }
    
    protected override void OnDisable()
    {
        CalledTutorial -= TutorialIntro;
    }

    private void TutorialIntro()
    {
        tutorial_1.Play();
    }    
    public void FadeOutLight(float duration)
    {
        StartCoroutine(FadeOutCoroutine(duration));
    }

    private IEnumerator FadeOutCoroutine(float duration)
    {
        float startIntensity = light.intensity;
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;
            light.intensity = Mathf.Lerp(startIntensity, 0f, t);
            yield return null;
        }

        light.intensity = 0f;
    }
    void OnTriggerExit(Collider other)
    {

    }
}
