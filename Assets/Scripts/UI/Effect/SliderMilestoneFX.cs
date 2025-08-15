using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class SliderMilestoneFX : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private Slider slider;
    [SerializeField] private Transform groupIcon;   // Parent chứa các icon mốc
    [SerializeField] private AudioSource sfxSource;

    [Header("Milestones (0..maxValue)")]
    [SerializeField] private List<float> milestoneValues = new();   // <-- float!
    [SerializeField] private bool triggerOnce = true;

    [Header("Bounce FX")]
    [SerializeField] private float bounceScale = 1.25f;
    [SerializeField] private float upTime = 0.12f, downTime = 0.18f;
    [SerializeField] private Ease upEase = Ease.OutBack, downEase = Ease.OutQuad;
    [SerializeField] private float hopY = 12f;

    [Header("SFX")]
    [SerializeField] private AudioClip defaultClip;
    [SerializeField] private bool randomizePitch = true;
    [SerializeField] private Vector2 pitchRange = new(0.96f, 1.06f);
    [SerializeField] private float minSfxInterval = 0.04f;

    [Header("Step Tween")]
    [SerializeField] private float stepDuration = 0.35f;
    [SerializeField] private Ease stepEase = Ease.OutCubic;
    [SerializeField] private bool useUnscaledTime = true;
    [SerializeField] private bool loopAtEnd = false;

    private readonly List<RectTransform> icons = new();
    private readonly List<bool> triggered = new();
    private float prevValue;
    private float lastSfxTime = -999f;
    private Tween stepTween;
    private const float EPS = 1e-4f;

    private void Awake()
    {
        if (!slider) slider = GetComponent<Slider>();
        if (!groupIcon)
        {
            var t = transform.Find("Group_Icon");
            if (t) groupIcon = t;
        }

        CacheIcons();
        BuildMilestonesIfEmpty();

        // đảm bảo milestones trong range & đã sort tăng dần
        for (int i = 0; i < milestoneValues.Count; i++)
            milestoneValues[i] = Mathf.Clamp(milestoneValues[i], slider.minValue, slider.maxValue);
        milestoneValues.Sort();

        prevValue = slider.value;
        slider.onValueChanged.AddListener(OnSliderValueChanged);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            StepToNext();
        }    
    }
    private void OnDestroy() => slider.onValueChanged.RemoveListener(OnSliderValueChanged);

    private void CacheIcons()
    {
        icons.Clear(); triggered.Clear();
        if (!groupIcon) return;

        for (int i = 0; i < groupIcon.childCount; i++)
        {
            if (groupIcon.GetChild(i) is RectTransform rt)
            {
                icons.Add(rt);
                triggered.Add(false);
                rt.localScale = Vector3.one;
            }
        }
    }

    private void BuildMilestonesIfEmpty()
    {
        if (icons.Count == 0) return;

        if (milestoneValues == null || milestoneValues.Count == 0)
        {
            // tự chia đều theo số icon và range slider
            float range = slider.maxValue - slider.minValue;
            int n = icons.Count;
            milestoneValues = new List<float>(n);
            for (int i = 1; i <= n; i++)
            {
                float t = (float)i / n; // 0..1
                milestoneValues.Add(slider.minValue + range * t);
            }
        }

        // Cắt về cùng số lượng với icon
        int count = Mathf.Min(milestoneValues.Count, icons.Count);
        if (milestoneValues.Count != count) milestoneValues.RemoveRange(count, milestoneValues.Count - count);
        if (icons.Count != count) icons.RemoveRange(count, icons.Count - count);
        if (triggered.Count != count)
        {
            triggered.Clear();
            for (int i = 0; i < count; i++) triggered.Add(false);
        }
    }

    private void OnSliderValueChanged(float cur)
    {
        if (icons.Count == 0) { prevValue = cur; return; }

        bool forward = cur >= prevValue;
        float a = forward ? prevValue : cur;
        float b = forward ? cur : prevValue;

        for (int i = 0; i < icons.Count; i++)
        {
            float thr = milestoneValues[i];
            // kiểm tra vượt qua mốc trong (a, b] (dùng epsilon để chắc ăn)
            if (thr > a + EPS && thr <= b + EPS)
            {
                if (!triggerOnce || !triggered[i])
                {
                    TriggerIcon(i);
                    triggered[i] = true;
                }
            }
        }

        prevValue = cur;
    }

    private void TriggerIcon(int index)
    {
        if (index < 0 || index >= icons.Count) return;
        var rt = icons[index];
        rt.DOKill(true);
        float startY = rt.anchoredPosition.y;

        var seq = DOTween.Sequence().SetUpdate(true);
        seq.Append(rt.DOScale(bounceScale, upTime).SetEase(upEase));
        if (Mathf.Abs(hopY) > EPS) seq.Join(rt.DOAnchorPosY(startY + hopY, upTime).SetEase(upEase));
        seq.Append(rt.DOScale(1f, downTime).SetEase(downEase));
        if (Mathf.Abs(hopY) > EPS) seq.Join(rt.DOAnchorPosY(startY, downTime).SetEase(downEase));

        PlayIconSfx();
    }

    private void PlayIconSfx()
    {
        if (!sfxSource || !defaultClip) return;
        float now = Time.unscaledTime;
        if (now - lastSfxTime < minSfxInterval) return;

        float old = sfxSource.pitch;
        if (randomizePitch) sfxSource.pitch = Random.Range(pitchRange.x, pitchRange.y);
        sfxSource.PlayOneShot(defaultClip);
        if (randomizePitch) sfxSource.pitch = old;

        lastSfxTime = now;
    }

    public void ResetTriggers(bool resetPrevToCurrent = true)
    {
        for (int i = 0; i < triggered.Count; i++) triggered[i] = false;
        if (resetPrevToCurrent) prevValue = slider.value;
    }

    // ---------- Step tới từng icon ----------
    public void StepToNext()
    {
        if (milestoneValues.Count == 0) return;

        float cur = slider.value;
        int idx = milestoneValues.FindIndex(v => v > cur + EPS);
        if (idx < 0)
        {
            if (loopAtEnd)
            {
                ResetTriggers(resetPrevToCurrent: false);
                StepToValue(milestoneValues[0]);
            }
            return;
        }
        StepToIndex(idx);
    }

    public void StepToPrev()
    {
        if (milestoneValues.Count == 0) return;

        float cur = slider.value;
        int idx = -1;
        for (int i = 0; i < milestoneValues.Count; i++)
            if (milestoneValues[i] < cur - EPS) idx = i;

        if (idx >= 0) StepToIndex(idx);
    }

    public void StepToIndex(int index)
    {
        if (index < 0 || index >= milestoneValues.Count) return;
        StepToValue(milestoneValues[index]);
    }

    public void StepToValue(float value)
    {
        value = Mathf.Clamp(value, slider.minValue, slider.maxValue);
        stepTween?.Kill();
        stepTween = DOTween.To(() => slider.value, v => slider.value = v, value, stepDuration)
                           .SetEase(stepEase)
                           .SetUpdate(useUnscaledTime);
    }

    // set lại mốc thủ công (float!)
    public void SetMilestones(List<float> absoluteValues, bool resetTriggers = true)
    {
        milestoneValues = new List<float>(absoluteValues);
        for (int i = 0; i < milestoneValues.Count; i++)
            milestoneValues[i] = Mathf.Clamp(milestoneValues[i], slider.minValue, slider.maxValue);
        milestoneValues.Sort();
        BuildMilestonesIfEmpty();
        if (resetTriggers) ResetTriggers();
    }
}
