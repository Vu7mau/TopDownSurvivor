using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoverScale3D : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private float scaleOnHover = 1.2f;  // Tỉ lệ phóng to
    [SerializeField] private float duration = 0.2f;      // Thời gian tween
    [SerializeField] private bool canHover = true;
    public bool CanHover { set => this.canHover = value; }
    private Vector3 originalScale;

    [SerializeField] protected List<AudioClip> snd_hovers;

    private void Awake()
    {
        originalScale = transform.localScale; // Lưu scale gốc
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(!this.canHover) return;
        this.PlayerHoverSoundFX();
        transform.DOScale(originalScale * scaleOnHover, duration)
            .SetEase(Ease.OutBack)
            .SetUpdate(true); // chạy ngay cả khi timeScale = 0
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!this.canHover) return;
        transform.DOScale(originalScale, duration)
            .SetEase(Ease.OutBack)
            .SetUpdate(true); // chạy ngay cả khi timeScale = 0
    }

    protected virtual void PlayerHoverSoundFX()
    {
        if (this.snd_hovers.Count == 0) return;
        int random = Random.Range(0,this.snd_hovers.Count);
        if(snd_hovers[random] != null)
        {
            SoundFXManager.Instance.PlaySoundFXClip(snd_hovers[random], this.transform);
        }
    }
}
