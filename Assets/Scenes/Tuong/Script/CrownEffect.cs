using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class CrownEffect : MonoBehaviour
{
    public static CrownEffect Instance;
    public RectTransform crownIcon;
    public Image lightStripe;
    public Image[] stars;
    public Image[] lightPetals;

    public float sizeCrownIcon = 1.1f;
    public float durationCrownIcon = 1f;
    public float lightStripeRotationSpeed = 10f;
    public float lightStripeFadeDuration = 4f;
    public float startAlpha = 0.3f;
    public float brightAlpha = 0.8f;
    public float lightPetalScaleDuration = 1.5f;
    public float starRotationAngle = 15f;
    public float starRotationDuration = 2f;

    public RectTransform parentPanel;
    public float slideDuration = 0.5f;
    public float slideOffset = 500f;

    private Vector3 crownOriginalScale;
    private Vector2 panelOriginalPos;

    // Để lưu trữ các Tween để dừng/kill khi cần
    private Tween crownScaleTween;
    private Tween lightStripeRotateTween;
    private Tween lightStripeFadeTween;
    private Tween[] starRotateTweens;
    private Tween[] starFadeTweens;
    private Tween[] petalScaleTweens;
    private Tween[] petalFadeTweens;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        if (parentPanel != null)
            panelOriginalPos = parentPanel.anchoredPosition;
    }

    public void Show()
    {
        gameObject.SetActive(true);

        if (parentPanel != null)
        {
            parentPanel.anchoredPosition = panelOriginalPos + Vector2.up * slideOffset;
            parentPanel.DOAnchorPos(panelOriginalPos, slideDuration).SetEase(Ease.OutCubic);
        }

        StartEffects();
    }

    public void Close()
    {
        if (parentPanel != null)
        {
            parentPanel.DOAnchorPos(panelOriginalPos + Vector2.up * slideOffset, slideDuration)
                .SetEase(Ease.InCubic)
                .OnComplete(() =>
                {
                    StopEffects();
                    gameObject.SetActive(false);
                });
        }
        else
        {
            StopEffects();
            gameObject.SetActive(false);
        }
    }

    void StartEffects()
    {
        crownOriginalScale = crownIcon.localScale;

        crownScaleTween = crownIcon.DOScale(crownOriginalScale * sizeCrownIcon, durationCrownIcon)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutSine);

        lightStripeRotateTween = lightStripe.rectTransform.DORotate(new Vector3(0, 0, lightStripeRotationSpeed), lightStripeFadeDuration)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutSine);

        lightStripeFadeTween = lightStripe.DOFade(brightAlpha, lightPetalScaleDuration)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutSine)
            .From(startAlpha);

        starRotateTweens = new Tween[stars.Length];
        starFadeTweens = new Tween[stars.Length];

        float currentStarRotationAngle = starRotationAngle;
        for (int i = 0; i < stars.Length; i++)
        {
            starRotateTweens[i] = stars[i].transform
                .DORotate(new Vector3(0, 0, currentStarRotationAngle), starRotationDuration)
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.InOutSine);

            currentStarRotationAngle = -currentStarRotationAngle;

            float minAlpha = Random.Range(0.3f, 0.5f);
            float maxAlpha = Random.Range(0.6f, 1f);
            float duration = Random.Range(1f, 2f);

            starFadeTweens[i] = stars[i].DOFade(maxAlpha, duration)
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.InOutSine)
                .From(minAlpha);
        }

        petalScaleTweens = new Tween[lightPetals.Length];
        petalFadeTweens = new Tween[lightPetals.Length];

        for (int i = 0; i < lightPetals.Length; i++)
        {
            float delay = i * 0.2f;
            float minScale = 0.9f;
            float maxScale = 1.1f;
            float duration = 1.5f;

            petalScaleTweens[i] = lightPetals[i].rectTransform
                .DOScale(maxScale, duration)
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.InOutSine)
                .SetDelay(delay)
                .From(minScale);

            float minAlpha = 0.3f;
            float maxAlpha = 0.7f;

            petalFadeTweens[i] = lightPetals[i]
                .DOFade(maxAlpha, duration)
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.InOutSine)
                .SetDelay(delay)
                .From(minAlpha);
        }
    }

    void StopEffects()
    {
        crownScaleTween?.Kill();
        lightStripeRotateTween?.Kill();
        lightStripeFadeTween?.Kill();

        if (starRotateTweens != null)
        {
            foreach (var t in starRotateTweens)
                t?.Kill();
        }
        if (starFadeTweens != null)
        {
            foreach (var t in starFadeTweens)
                t?.Kill();
        }

        if (petalScaleTweens != null)
        {
            foreach (var t in petalScaleTweens)
                t?.Kill();
        }
        if (petalFadeTweens != null)
        {
            foreach (var t in petalFadeTweens)
                t?.Kill();
        }

        ResetToInitialState();
    }

    void ResetToInitialState()
    {
        crownIcon.localScale = crownOriginalScale;
        lightStripe.rectTransform.localRotation = Quaternion.identity;
        Color lightStripeColor = lightStripe.color;
        lightStripeColor.a = startAlpha;
        lightStripe.color = lightStripeColor;

        foreach (var star in stars)
        {
            star.transform.localRotation = Quaternion.identity;
            Color c = star.color;
            c.a = 1f;
            star.color = c;
        }

        foreach (var petal in lightPetals)
        {
            petal.rectTransform.localScale = Vector3.one;
            Color c = petal.color;
            c.a = 1f;
            petal.color = c;
        }
    }
}
