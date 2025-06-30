using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class PlayButtonEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    public Outline outline;         
    public AudioSource audioSource; 
    public Color hoverOutlineColor = new Color32(255, 215, 0, 200); 
    public float scaleDown = 0.95f;
    public float scaleTime = 0.1f;

    private RectTransform rectTransform;
    private Vector3 originalScale;
    private Color originalOutlineColor;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        originalScale = rectTransform.localScale;

        if (outline != null)
            originalOutlineColor = outline.effectColor;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (outline != null)
            outline.effectColor = hoverOutlineColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (outline != null)
            outline.effectColor = originalOutlineColor;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        rectTransform.DOKill(); 
        rectTransform.DOScale(originalScale * scaleDown, scaleTime)
            .OnComplete(() => rectTransform.DOScale(originalScale, scaleTime));

        if (audioSource != null)
            audioSource.Play();
    }
}
