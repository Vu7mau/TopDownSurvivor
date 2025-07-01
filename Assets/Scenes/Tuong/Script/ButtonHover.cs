using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.Audio;
public class ButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler,
    IPointerDownHandler, IPointerUpHandler
{
    [Header("Components")]
    public Image targetImage;
    public AudioSource clickSFX;
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
    }
    private void OnDisable()
    {
        rectTransform?.DOKill();
        targetImage?.DOKill();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        rectTransform.DOScale(originalScale * hoverScale, scaleTime)
             .SetEase(Ease.OutBack);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        rectTransform.DOScale(originalScale, scaleTime)
             .SetEase(Ease.OutBack);
    }
    public void OnPointerDown(PointerEventData evt)
    {
        rectTransform.DOScale(originalScale * pressedScale, scaleTime);
        float buttonVolume;
        if(mixer.GetFloat("Button", out buttonVolume))
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
        rectTransform.DOScale(targetScale, scaleTime)
             .SetEase(Ease.OutBack);
    }
}
