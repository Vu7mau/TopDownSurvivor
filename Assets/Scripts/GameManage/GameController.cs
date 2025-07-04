using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameController : Singleton<GameController>
{
    [SerializeField] private List<Map_Controller> maps; // Danh sách map
                                                   
    [SerializeField] private Map_Controller currentMap; // Map hiện tại
    [SerializeField] private Map_Controller lastMap; // Map hiện tại


    public Action OnMapSwitched; // Sự kiện thông báo chuyển map
    public Action OnWaveStarted; // Sự kiện thông báo bắt đầu wave

    [SerializeField] private Transform character;


    [SerializeField] private Image fadeImage; // kéo Image vào đây từ Inspector
    [SerializeField] private float fadeDuration = 1.0f;

    private bool isFadeInComplete = false;

    protected override void Start()
    {
        base.Start();
        StartCoroutine(FadeIn());

    }
    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadCharacter();
    }
    private void LoadCharacter()
    {
        if (character == null)
        {
            character = GameObject.FindObjectOfType<CharacterCtrl>()?.transform;
        }
    }
    protected override void OnEnable()
    {
        if(currentMap == null)
        {
            currentMap = maps.FirstOrDefault();
        }
    }
    public void MoveCharacterPos(Transform pos)
    {
        if (character != null && pos != null)
        {
            character.SetPositionAndRotation(pos.position, pos.rotation);
        }
        else
        {
            Debug.LogError("Character hoặc pos không được gán!");
        }
    }

    public void ScreenFadeIn()
    {
        StartCoroutine(FadeIn());

    }
    public void ScreenFadeOut()
    {
        StartCoroutine(FadeOut());
    }
    public void SwitchMap(int mapIndex)
    {

        if (mapIndex < 0 || mapIndex >= maps.Count)
        {
            Debug.LogError($"Map index {mapIndex} không hợp lệ!");
            return;
        }

        lastMap=currentMap;
        // Bật map mới
        currentMap = maps[mapIndex];
        currentMap.map.gameObject.SetActive(true);
        currentMap.EnableProcessing();
        StartCoroutine(FadeOut());


        // Thông báo chuyển map
        OnMapSwitched?.Invoke();
        CharacterUIManager.OnScreenFadeIn?.Invoke();
         StartCoroutine(FadeIn());


    }
    private IEnumerator FadeIn()
    {
        yield return new WaitUntil(() => isFadeInComplete); // Chờ FadeIn hoàn thành
        float t = 0f;
        Color color = fadeImage.color;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            color.a = Mathf.Lerp(1f, 0f, t / fadeDuration);
            fadeImage.color = color;
            yield return null;
        }
        color.a = 0f;
        fadeImage.color = color;
    }

    private IEnumerator FadeOut()
    {
        isFadeInComplete = false;
        float t = 0f;
        Color color = fadeImage.color;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            color.a = Mathf.Lerp(0f, 1f, t / fadeDuration);
            fadeImage.color = color;
            yield return null;
        }

        color.a = 1f;
        fadeImage.color = color;
        isFadeInComplete = true;
        yield return isFadeInComplete;
        MoveCharacterPos(currentMap.currentMapSpawnPoint);
        // Tắt map cũ
        if (lastMap != null)
        {
            lastMap.map.gameObject.SetActive(false);
            lastMap.DisableProcessing();
        }


    }
}
