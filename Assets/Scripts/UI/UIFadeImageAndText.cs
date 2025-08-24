using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIFadeImageAndText : MonoBehaviour
{
    public enum TimeMode { GameTime, UnscaledTime }
    public enum OutOrder { TextFirstThenImage, ImageOnly, ImageFirstThenText }

    [Header("References")]
    [Tooltip("Ảnh cần fade. Nếu để trống, script sẽ cố tự tìm Image trên cùng GameObject.")]
    public Image targetImage;
    [Tooltip("Text cần fade. Có thể để null nếu không dùng.")]
    public TextMeshProUGUI tmpText;
    [Tooltip("Text thường (UI.Text). Nếu dùng TMP ở trên thì để trống mục này.")]
    public Text legacyText;

    [Header("Behaviour")]
    [Tooltip("Tự động fade in khi OnEnable.")]
    public bool playOnEnable = true;
    [Tooltip("Khi fade out xong có tự SetActive(false) object này không.")]
    public bool deactivateAfterFadeOut = false;

    [Header("Timing")]
    [Tooltip("Thời gian fade cho Image (giây).")]
    public float imageFadeDuration = 0.6f;
    [Tooltip("Thời gian fade cho Text (giây).")]
    public float textFadeDuration = 0.45f;
    [Tooltip("Delay trước khi Image bắt đầu fade in (giây).")]
    public float imageStartDelay = 0f;
    [Tooltip("Delay bắt đầu fade Text sau khi Image đã đầy 1.0 (giây).")]
    public float textDelayAfterImageFull = 0.05f;
    [Tooltip("Chế độ thời gian cho fade.")]
    public TimeMode timeMode = TimeMode.GameTime;

    [Header("Order & Easing")]
    [Tooltip("Thứ tự khi fade OUT.")]
    public OutOrder outOrder = OutOrder.TextFirstThenImage;
    [Tooltip("Đường cong easing cho Image.")]
    public AnimationCurve imageEase = AnimationCurve.EaseInOut(0, 0, 1, 1);
    [Tooltip("Đường cong easing cho Text.")]
    public AnimationCurve textEase = AnimationCurve.EaseInOut(0, 0, 1, 1);

    [Header("Debug / Defaults")]
    [Tooltip("Khi Awake, ép Image alpha = 0 & Text alpha = 0 (chuẩn bị cho fade in).")]
    public bool forceZeroOnAwake = true;

    // state
    private Coroutine _playing;
    private float _imgStartAlpha = 0f;
    private float _txtStartAlpha = 0f;

    void Reset()
    {
        targetImage = GetComponent<Image>();
        tmpText = GetComponentInChildren<TextMeshProUGUI>(true);
        if (!tmpText) legacyText = GetComponentInChildren<Text>(true);
    }

    void Awake()
    {
        if (!targetImage) targetImage = GetComponent<Image>();
        // set alpha 0 nếu yêu cầu
        if (forceZeroOnAwake)
        {
            if (targetImage) SetImageAlpha(0f);
            SetTextAlpha(0f);
        }
    }

    void OnEnable()
    {
        if (playOnEnable)
        {
            // Fade in: Image 0→1, rồi Text 0→1
            PlayIn();
        }
    }

    void OnDisable()
    {
        // Lưu ý: nếu bạn tắt GameObject bên ngoài ngay lập tức thì coroutine không chạy được.
        // Hãy dùng PlayOut() hoặc FadeOutAndDeactivate() để đảm bảo có hiệu ứng trước khi tắt.
    }

    // -------- Public API --------

    /// <summary>Chạy hiệu ứng fade in: Image 0→1, xong mới Text 0→1.</summary>
    public void PlayIn()
    {
        KillPlaying();
        _playing = StartCoroutine(Co_FadeIn());
    }

    /// <summary>Chạy hiệu ứng fade out (thứ tự tùy chọn in Inspector).</summary>
    public void PlayOut()
    {
        KillPlaying();
        _playing = StartCoroutine(Co_FadeOut(deactivateAfterFadeOut));
    }

    /// <summary>Fade out rồi tắt object (SetActive(false)).</summary>
    public void FadeOutAndDeactivate()
    {
        KillPlaying();
        _playing = StartCoroutine(Co_FadeOut(true));
    }

    // -------- Coroutines --------

    private IEnumerator Co_FadeIn()
    {
        // chuẩn bị
        if (targetImage) _imgStartAlpha = Mathf.Clamp01(GetImageAlpha());
        _txtStartAlpha = Mathf.Clamp01(GetTextAlpha());

        // đảm bảo Image bắt đầu từ 0 (hoặc từ giá trị hiện tại nếu bạn muốn)
        if (targetImage) SetImageAlpha(0f);
        SetTextAlpha(0f);

        // optional delay trước khi image bắt đầu
        if (imageStartDelay > 0f) yield return WaitFor(imageStartDelay);

        // Image 0→1
        if (targetImage && imageFadeDuration > 0f)
            yield return Lerp01(imageFadeDuration, a => SetImageAlpha(ImageEase(a)));
        else if (targetImage) SetImageAlpha(1f);

        // đợi một chút rồi Text 0→1
        if (textDelayAfterImageFull > 0f) yield return WaitFor(textDelayAfterImageFull);

        if (textFadeDuration > 0f)
            yield return Lerp01(textFadeDuration, a => SetTextAlpha(TextEase(a)));
        else
            SetTextAlpha(1f);

        _playing = null;
    }

    private IEnumerator Co_FadeOut(bool deactivateAtEnd)
    {
        // Nếu không có Image hoặc Text, vẫn hoạt động bình thường
        switch (outOrder)
        {
            case OutOrder.TextFirstThenImage:
                // Text → 0
                if (textFadeDuration > 0f)
                    yield return Lerp01(textFadeDuration, a => SetTextAlpha(TextEase(1f - a)));
                else
                    SetTextAlpha(0f);
                // Image → 0
                if (targetImage && imageFadeDuration > 0f)
                    yield return Lerp01(imageFadeDuration, a => SetImageAlpha(ImageEase(1f - a)));
                else if (targetImage) SetImageAlpha(0f);
                break;

            case OutOrder.ImageOnly:
                if (targetImage && imageFadeDuration > 0f)
                    yield return Lerp01(imageFadeDuration, a => SetImageAlpha(ImageEase(1f - a)));
                else if (targetImage) SetImageAlpha(0f);
                break;

            case OutOrder.ImageFirstThenText:
                if (targetImage && imageFadeDuration > 0f)
                    yield return Lerp01(imageFadeDuration, a => SetImageAlpha(ImageEase(1f - a)));
                else if (targetImage) SetImageAlpha(0f);

                if (textFadeDuration > 0f)
                    yield return Lerp01(textFadeDuration, a => SetTextAlpha(TextEase(1f - a)));
                else
                    SetTextAlpha(0f);
                break;
        }

        _playing = null;

        if (deactivateAtEnd)
            gameObject.SetActive(false);
    }

    // -------- Small helpers --------

    private void KillPlaying()
    {
        if (_playing != null) StopCoroutine(_playing);
        _playing = null;
    }

    private float GetImageAlpha()
    {
        if (!targetImage) return 0f;
        return targetImage.color.a;
    }

    private void SetImageAlpha(float a)
    {
        if (!targetImage) return;
        var c = targetImage.color;
        c.a = Mathf.Clamp01(a);
        targetImage.color = c;
    }

    private float GetTextAlpha()
    {
        if (tmpText) return tmpText.alpha;
        if (legacyText) return legacyText.color.a;
        return 0f;
    }

    private void SetTextAlpha(float a)
    {
        a = Mathf.Clamp01(a);
        if (tmpText)
        {
            tmpText.alpha = a;
        }
        if (legacyText)
        {
            var c = legacyText.color;
            c.a = a;
            legacyText.color = c;
        }
    }

    private float ImageEase(float t) => imageEase != null ? imageEase.Evaluate(Mathf.Clamp01(t)) : t;
    private float TextEase(float t) => textEase != null ? textEase.Evaluate(Mathf.Clamp01(t)) : t;

    private IEnumerator Lerp01(float duration, System.Action<float> onStep)
    {
        if (duration <= 0f)
        {
            onStep?.Invoke(1f);
            yield break;
        }

        float t = 0f;
        while (t < duration)
        {
            t += Delta();
            float k = Mathf.Clamp01(t / duration);
            onStep?.Invoke(k);
            yield return null;
        }
        onStep?.Invoke(1f);
    }

    private float Delta()
    {
        return timeMode == TimeMode.GameTime ? Time.deltaTime : Time.unscaledDeltaTime;
    }

    private WaitForSeconds WaitFor(float seconds)
    {
        // nếu UnscaledTime thì không dùng WaitForSeconds (vì nó dùng scaled time)
        return timeMode == TimeMode.GameTime ? new WaitForSeconds(seconds) : null;
    }

    private IEnumerator WaitFor(float seconds, bool useUnscaled)
    {
        if (!useUnscaled) yield return new WaitForSeconds(seconds);
        else
        {
            float t = 0f;
            while (t < seconds)
            {
                t += Time.unscaledDeltaTime;
                yield return null;
            }
        }
    }
}
