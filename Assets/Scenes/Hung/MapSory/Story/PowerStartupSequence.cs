using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PowerStartupSequence : MonoBehaviour
{
    public float startupTime = 60f;
    public Slider countdownSlider;
    public GameEventManager gameEventManager;
    public List<MonsterSpawnerTrigger> monsterSpawners;

    [SerializeField] private AudioClip notificationAudi;
    [SerializeField] private float delaySpawn = 1f;
    [SerializeField] private Image fillImage; // kéo Fill image của Slider vào đây trong Inspector
    [SerializeField] private float smoothDuration = 0.3f;

    private bool isRunning = false;

    private void Start()
    {
        if (countdownSlider != null)
            countdownSlider.gameObject.SetActive(false);
    }

    public void StartSequence()
    {
        if (isRunning) return;

        StartCoroutine(RunStartup());
    }

    private IEnumerator RunStartup()
    {
        isRunning = true;

        // Hiện lời thoại bắt đầu
        string content = "Hệ thống đang khởi động, vui lòng chờ 1 phút...";
        ChatDialogueManager.Instance.chatDialogue.ShowDialogue(content, 10,notificationAudi, "Hệ thống");
        BackgroundMusicManager.Instance.PlayMusic(BackgroundMusicManager.Instance.musicClip_3);
        if (countdownSlider != null)
        {
            countdownSlider.gameObject.SetActive(true);
            countdownSlider.maxValue = startupTime;
            countdownSlider.value = 0;
            UpdateFillColor(0f);
        }

        var wait = new WaitForSeconds(delaySpawn);

        foreach (var spawner in monsterSpawners)
        {
            yield return wait;
            if (spawner != null)
                spawner.SpawnWave(4);
        }

        float elapsed = 0f;
        while (elapsed < startupTime)
        {
            elapsed += Time.deltaTime;

            if (countdownSlider != null)
            {
                AnimateSlider(elapsed);
                UpdateFillColor(elapsed / startupTime);
            }

            yield return null;
        }

        if (countdownSlider != null)
            countdownSlider.gameObject.SetActive(false);

        string content2 = "Nguồn điện đã được khởi động lại!";
        ChatDialogueManager.Instance.chatDialogue.ShowDialogue(content2, 10, notificationAudi, "Hệ thống");
        gameEventManager.FinishPowerActivation();

        isRunning = false;
    }

    private void AnimateSlider(float value)
    {
        if (countdownSlider != null)
        {
            countdownSlider.DOValue(value, smoothDuration).SetEase(Ease.OutSine);
        }
    }

    private void UpdateFillColor(float percent)
    {
        if (fillImage != null)
        {
            fillImage.color = Color.Lerp(Color.red, Color.green, percent);
        }
    }


}
