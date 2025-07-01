using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngineInternal;
using UnityEditor.Localization.Plugins.XLIFF.V12;
using UnityEngine.Audio;

public class PlayButtonEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, 
    IPointerDownHandler, IPointerUpHandler
{
    [Header("Components")]
    public Outline outline;
    public Image targetImage;
    public AudioSource clickSFX; 
    [Header("Color")]
    public Color hoverOutlineColor = new Color32(255, 215, 0, 200); 
    public Color hoverTintColor = new Color32(255, 255, 255, 200);
    private Color originalOutlineColor;
    private Color originalTintColor;
    [Header("Scale Settings")]
    public float hoverScale = 1.05f;
    public float pressedScale = 0.95f; 
    public float scaleTime = 0.1f;
    public AudioMixer mixer;
    private RectTransform rectTransform;
    private Vector3 originalScale;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        originalScale = rectTransform.localScale;

        if (outline != null)
            originalOutlineColor = outline.effectColor;
        if (targetImage != null)
            originalTintColor = targetImage.color;
    }
    private void OnDisable()
    {
        rectTransform?.DOKill();
        outline?.DOKill();
        targetImage?.DOKill();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        outline?.DOColor(hoverOutlineColor, 0.2f);
        targetImage?.DOColor(hoverTintColor, 0.2f);
        rectTransform.DOScale(originalScale * hoverScale, scaleTime)
             .SetEase(Ease.OutBack);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        outline?.DOColor(originalOutlineColor, 0.2f);
        targetImage?.DOColor(originalTintColor, 0.2f);
        rectTransform.DOScale(originalScale, scaleTime)
             .SetEase(Ease.OutBack);
    }
    public void OnPointerDown(PointerEventData evt)
    {
        rectTransform.DOKill(true);
        outline?.DOKill();
        targetImage?.DOKill();

        rectTransform.DOScale(originalScale * pressedScale, scaleTime);
        float buttonVolume;
        if (mixer.GetFloat("Button", out buttonVolume))
        {
            float volume = Mathf.Pow(10f, buttonVolume / 20f);
            clickSFX.volume = volume;
        }
        else
        {
            clickSFX.volume = 1f;
        }
        clickSFX.Play();
    }
    public void OnPointerUp(PointerEventData evt)
    {
        bool isHover = RectTransformUtility.RectangleContainsScreenPoint(
            rectTransform, evt.position, evt.enterEventCamera);

        Vector3 targetScale = originalScale * (isHover ? hoverScale : 1f);
        Color targetTint = isHover ? hoverTintColor : originalTintColor;
        Color targetOutln = isHover ? hoverOutlineColor : originalOutlineColor;

        rectTransform.DOScale(targetScale, scaleTime)
             .SetEase(Ease.OutBack);
        targetImage?.DOColor(targetTint, scaleTime);
        outline?.DOColor(targetOutln, 0.2f);
    }
}
