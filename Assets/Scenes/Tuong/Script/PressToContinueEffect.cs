using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class PressToContinueEffect : MonoBehaviour
{
    public static PressToContinueEffect Instance;
    [Header("UI References")]
    [SerializeField] private Graphic targetText;

    [Header("Fade Settings")]
    [SerializeField] private float fadeMinAlpha = 0.3f;
    [SerializeField] private float fadeMaxAlpha = 1f;
    [SerializeField] private float fadeDuration = 1f;

    [Header("Pulse Settings")]
    [SerializeField] private float pulseScale = 1.05f;
    [SerializeField] private float pulseDuration = 1f;

    private Vector3 originalScale;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        if (targetText == null)
            targetText = GetComponent<Graphic>();

        originalScale = targetText.rectTransform.localScale;
    }
    public void PlayEffect()
    {
        targetText.DOFade(fadeMinAlpha, fadeDuration)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutSine);

        targetText.rectTransform.DOScale(originalScale * pulseScale, pulseDuration)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutSine);
    }
}
