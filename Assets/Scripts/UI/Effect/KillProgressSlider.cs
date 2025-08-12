using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class KillProgressSlider : Singleton<KillProgressSlider>
{
    [Header("Refs")]
    [SerializeField] private Slider slider;
    [SerializeField] private RectTransform popTarget;     // Kéo Fill (RectTransform) của Slider vào đây để pop đẹp
    [SerializeField] private TMP_Text valueText;          // Hiển thị "current/max"

    [Header("Value Tween")]
    [SerializeField] private float durationPerUnit = 0.12f; // giây cho mỗi 1 đơn vị (1 kill)
    [SerializeField] private float minDuration = 0.05f;
    [SerializeField] private float maxDuration = 0.25f;
    [SerializeField] private Ease valueEase = Ease.OutQuad;
    [SerializeField] private bool useUnscaledTime = true;

    [Header("Pop FX")]
    [SerializeField] private float popScale = 1.08f;
    [SerializeField] private float popOut = 0.07f;
    [SerializeField] private float popBack = 0.10f;
    [SerializeField] private Ease popOutEase = Ease.OutBack;
    [SerializeField] private Ease popBackEase = Ease.InQuad;

    [Header("Sound")]
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioClip tickClip;
    [SerializeField, Range(0f, 1f)] private float tickVolume = 1f;
    [SerializeField] private bool randomizePitch = true;
    [SerializeField] private Vector2 pitchRange = new Vector2(0.96f, 1.06f);
    [SerializeField] private float minTickInterval = 0.05f;  // chống spam

    [Header("Events")]
    public UnityEvent onFilled; // gọi khi chạm max lần đầu

    // State
    private float targetValue;
    private Tween valueTween, popTween;
    private float lastTickTime = -999f;
    private bool firedFilledEvent = false;

    void Awake()
    {
        if (!slider) slider = GetComponentInChildren<Slider>();
        if (!popTarget) popTarget = slider != null ? slider.fillRect as RectTransform : (transform as RectTransform);
        if (!sfxSource) sfxSource = GetComponent<AudioSource>();

        // mặc định dùng số nguyên cho kill
        if (slider) slider.wholeNumbers = true;

        targetValue = slider ? Mathf.Clamp(slider.value, slider.minValue, slider.maxValue) : 0f;
        if (popTarget) popTarget.localScale = Vector3.one;
        UpdateLabel();
    }

    /* ---------------------- API cấu hình ---------------------- */

    /// <summary>Đặt max (goal). min = 0. Có thể reset current.</summary>
    public void SetMax(int max, bool resetCurrent = true, int startValue = 0)
    {
        if (!slider) return;
        slider.minValue = 0;
        slider.maxValue = Mathf.Max(1, max);
        firedFilledEvent = false;

        if (resetCurrent) SetImmediate(startValue);
        else UpdateLabel();
    }

    /// <summary>Đặt ngay mà không tween (sync trước trận).</summary>
    public void SetImmediate(int absoluteValue)
    {
        if (!slider) return;
        targetValue = Mathf.Clamp(absoluteValue, (int)slider.minValue, (int)slider.maxValue);
        KillValueTween();
        slider.value = targetValue;
        KillPopTween();
        if (popTarget) popTarget.localScale = Vector3.one;
        UpdateLabel();
        CheckFilledEvent();
    }

    /* ---------------------- API cộng tiến độ ---------------------- */

    /// <summary>Cộng theo số kill (đơn vị tuyệt đối). Gọi khi 1 con quái bị giết.</summary>
    public void AddKill(int count = 1)
    {
        Add(count);
    }

    /// <summary>Cộng delta tuyệt đối.</summary>
    public void Add(float delta)
    {
        if (!slider || Mathf.Approximately(delta, 0f)) return;

        float newTarget = Mathf.Clamp(targetValue + delta, slider.minValue, slider.maxValue);
        ApplyTarget(newTarget, playSfx: delta > 0f, playPop: delta > 0f);
    }

    /* ---------------------- Core ---------------------- */

    private void ApplyTarget(float newTarget, bool playSfx, bool playPop)
    {
        if (!slider) return;

        float from = slider.value;
        targetValue = newTarget;
        float deltaAbs = Mathf.Abs(targetValue - from);
        float dur = Mathf.Clamp(deltaAbs * durationPerUnit, minDuration, maxDuration);

        KillValueTween();
        valueTween = DOTween.To(
                () => slider.value,
                v => { slider.value = v; UpdateLabel(); },
                targetValue,
                dur)
            .SetEase(valueEase)
            .SetUpdate(useUnscaledTime)
            .OnComplete(CheckFilledEvent);

        if (playPop && popTarget)
        {
            KillPopTween();
            popTween = DOTween.Sequence().SetUpdate(useUnscaledTime)
                .Append(popTarget.DOScale(popScale, popOut).SetEase(popOutEase))
                .Append(popTarget.DOScale(1f, popBack).SetEase(popBackEase));
        }

        if (playSfx) PlayTick();
    }

    private void CheckFilledEvent()
    {
        if (!slider) return;
        if (!firedFilledEvent && Mathf.Approximately(slider.value, slider.maxValue))
        {
            firedFilledEvent = true;
            onFilled?.Invoke();
        }
    }

    private void PlayTick()
    {
        if (!tickClip || !sfxSource) return;
        float now = useUnscaledTime ? Time.unscaledTime : Time.time;
        if (now - lastTickTime < minTickInterval) return;

        float oldPitch = sfxSource.pitch;
        if (randomizePitch) sfxSource.pitch = Random.Range(pitchRange.x, pitchRange.y);
        sfxSource.PlayOneShot(tickClip, tickVolume);
        if (randomizePitch) sfxSource.pitch = oldPitch;

        lastTickTime = now;
    }

    private void UpdateLabel()
    {
        if (!valueText || !slider) return;
        int cur = Mathf.RoundToInt(slider.value);
        int max = Mathf.RoundToInt(slider.maxValue);
        valueText.text = $"{cur}/{max}";
    }

    private void KillValueTween()
    {
        if (valueTween != null && valueTween.IsActive()) valueTween.Kill();
        valueTween = null;
    }

    private void KillPopTween()
    {
        if (popTween != null && popTween.IsActive()) popTween.Kill();
        popTween = null;
    }

    /* ---------------------- tiện ích ---------------------- */

    public int Current => slider ? Mathf.RoundToInt(slider.value) : 0;
    public int Max => slider ? Mathf.RoundToInt(slider.maxValue) : 0;
}
