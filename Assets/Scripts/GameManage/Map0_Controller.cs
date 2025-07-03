using System.Collections;
using System.Collections.Generic;
using UnityEditor.Localization.Plugins.XLIFF.V20;
using UnityEngine;
using UnityEngine.Rendering;

public class Map0_Controller : Map_Controller
{
    [SerializeField] private Light light;
   

   
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameController.SwitchMap(mapIndexNextTo);
            this.processing.gameObject.SetActive(false);
            //CharacterUIManager.OnScreenFadeOut?.Invoke();
            //mapDisable.gameObject.SetActive(false);
            //Debug.Log("Enter map");
             FadeOutLight(2);
        }
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
