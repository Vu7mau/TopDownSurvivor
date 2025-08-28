using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class PowerStartupSequence : MonoBehaviour
{
    public float startupTime = 60f;
    public Slider countdownSlider;
    public GameEventManager gameEventManager;
    public List<MonsterSpawnerTrigger> monsterSpawners;

    [SerializeField] private AudioClip notificationAudi;
    [SerializeField] private AudioClip warningAudio;
    [SerializeField] private float delaySpawn = 1f;
    [SerializeField] private Image fillImage; // kéo Fill image của Slider vào đây trong Inspector
    [SerializeField] private float smoothDuration = 0.3f;

    [SerializeField] private Transform wave2;

    private bool isRunning = false;

    // NEW: Sự kiện khi bật điện thành công
    [Header("Events")]
    public UnityEvent onPowerStartupSuccess;

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
        wave2.gameObject.SetActive(false);

        // Hiện lời thoại bắt đầu
        string content = "Hệ thống đang khởi động, vui lòng chờ 1 phút...";
        ChatDialogueManager.Instance.chatDialogue.ShowDialogue(content, 10, notificationAudi, "Hệ thống");

        if (countdownSlider != null)
        {
            countdownSlider.gameObject.SetActive(true);
            countdownSlider.maxValue = startupTime;
            countdownSlider.value = 0;
            UpdateFillColor(0f);
        }

        var audi = SoundFXManager.Instance.PlaySoundFXClip(warningAudio, this.transform);
        var wait = new WaitForSeconds(delaySpawn);

        foreach (var spawner in monsterSpawners)
        {
            yield return wait;
            if (spawner != null)
                spawner.Spawn(2);
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

        // --- Thành công ---
        string content2 = "Nguồn điện đã được khởi động lại!";
        ChatDialogueManager.Instance.chatDialogue.ShowDialogue(content2, 10, notificationAudi, "Hệ thống");

        CharacterCtrl.Instance.CharacterEffect.TurnOffLight();

        if (countdownSlider != null)
            countdownSlider.gameObject.SetActive(false);

        audi.volume = 0;
        isRunning = false;

        wave2.gameObject.SetActive(true);
        BackgroundMusicManager.Instance.PlayMusic(BackgroundMusicManager.Instance.musicClip_3);

        // GỌI EVENT thành công (Inspector có thể bind thêm hành vi)
        onPowerStartupSuccess?.Invoke();
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
