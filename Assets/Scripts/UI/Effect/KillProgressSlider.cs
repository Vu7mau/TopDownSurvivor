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
    [SerializeField] private RectTransform popTarget;     // Fill (RectTransform)
    [SerializeField] private TMP_Text valueText;          // "current/max"

    [Header("Value Tween")]
    [SerializeField] private float durationPerUnit = 0.12f;
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
    [SerializeField] private float minTickInterval = 0.05f;
    private static float s_lastGlobalTickTime = -999f; // rate limit toàn cục
    [SerializeField] private float minGlobalTickInterval = 0.02f;

    [Header("Events")]
    public UnityEvent onFilled;

    // State
    private float targetValue;
    private Tween valueTween, popTween;
    private float lastTickTime = -999f;
    private bool firedFilledEvent = false;
    private int lastDisplayedValue = int.MinValue;

    protected override void Awake()
    {
        if (!slider) slider = GetComponentInChildren<Slider>();
        if (!popTarget && slider) popTarget = slider.fillRect as RectTransform;
        if (!sfxSource) sfxSource = GetComponent<AudioSource>();

        if (slider) slider.wholeNumbers = true;

        targetValue = slider ? slider.value : 0f;
        if (popTarget) popTarget.localScale = Vector3.one;
        UpdateLabel();
        SetMax(999, resetCurrent: true);
    }

    // ===== API =====

    public void SetMax(int max, bool resetCurrent = true, int startValue = 0)
    {
        if (!slider) return;
        slider.minValue = 0;
        slider.maxValue = Mathf.Max(1, max);
        firedFilledEvent = false;
        if (resetCurrent) SetImmediate(startValue);
        else UpdateLabel();
    }

    public void SetImmediate(int absoluteValue)
    {
        if (!slider) return;
        targetValue = Mathf.Clamp(absoluteValue, slider.minValue, slider.maxValue);
        KillValueTween();
        slider.value = targetValue;
        KillPopTween();
        if (popTarget) popTarget.localScale = Vector3.one;
        UpdateLabel();
        CheckFilledEvent();
    }

    public void AddKill(int count = 1) => Add(count);

    public void Add(float delta)
    {
        if (!slider || Mathf.Approximately(delta, 0f)) return;

        // Cộng vào targetValue
        targetValue = Mathf.Clamp(targetValue + delta, slider.minValue, slider.maxValue);

        // Kill tween cũ và tween từ giá trị hiện tại
        KillValueTween();
        float from = slider.value;
        float dur = Mathf.Clamp(Mathf.Abs(targetValue - from) * durationPerUnit, minDuration, maxDuration);

        valueTween = DOTween.To(
            () => slider.value,
            v =>
            {
                slider.value = v;
                int curInt = Mathf.RoundToInt(v);
                if (curInt != lastDisplayedValue)
                {
                    lastDisplayedValue = curInt;
                    UpdateLabelFast(curInt, Mathf.RoundToInt(slider.maxValue));
                }
                if (!firedFilledEvent && Mathf.Approximately(slider.value, slider.maxValue))
                    CheckFilledEvent();
            },
            targetValue,
            dur
        ).SetEase(valueEase)
         .SetUpdate(useUnscaledTime)
         .OnComplete(CheckFilledEvent);

        if (delta > 0f) PlayPop();
        if (delta > 0f) PlayTick();
    }

    // ===== FX =====

    private void PlayPop()
    {
        if (!popTarget) return;
        KillPopTween();
        popTween = DOTween.Sequence().SetUpdate(useUnscaledTime)
            .Append(popTarget.DOScale(popScale, popOut).SetEase(popOutEase))
            .Append(popTarget.DOScale(1f, popBack).SetEase(popBackEase));
    }

    private void PlayTick()
    {
        if (!tickClip || !sfxSource) return;
        float now = useUnscaledTime ? Time.unscaledTime : Time.time;

        if (now - lastTickTime < minTickInterval) return;
        if (now - s_lastGlobalTickTime < minGlobalTickInterval) return;

        float oldPitch = sfxSource.pitch;
        if (randomizePitch) sfxSource.pitch = Random.Range(pitchRange.x, pitchRange.y);
        sfxSource.PlayOneShot(tickClip, tickVolume);
        if (randomizePitch) sfxSource.pitch = oldPitch;

        lastTickTime = now;
        s_lastGlobalTickTime = now;
    }

    // ===== Helpers =====

    private void UpdateLabel()
    {
        if (!valueText || !slider) return;
        int cur = Mathf.RoundToInt(slider.value);
        int max = Mathf.RoundToInt(slider.maxValue);
        valueText.text = $"{cur}/{max}";
        lastDisplayedValue = cur;
    }

    private void UpdateLabelFast(int cur, int max)
    {
        if (!valueText) return;
        valueText.text = cur.ToString() + "/" + max.ToString();
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

    public int Current => slider ? Mathf.RoundToInt(slider.value) : 0;
    public int Max => slider ? Mathf.RoundToInt(slider.maxValue) : 0;
}
