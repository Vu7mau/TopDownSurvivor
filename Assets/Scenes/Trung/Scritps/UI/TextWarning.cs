using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class TextWarning : MonoBehaviour
{
    [SerializeField] private Color textColor;
    [SerializeField] private float duration;
    [SerializeField] private int timeLoop;
    [SerializeField] private TextMeshProUGUI textMeshPro;
    [SerializeField] private AudioClip SFX;
    private void Awake()
    {
        textMeshPro = GetComponent<TextMeshProUGUI>();
    }
    private void Start()
    {
        FadeTextRoutine();
    }
    private void FadeTextRoutine()
    {
        if (textMeshPro == null) return;
        textMeshPro.color = textColor;
        Color startColor = textMeshPro.color;
        Color transparentColor = new Color(startColor.r, startColor.g, startColor.b, 0);
        PlaySound();
        textMeshPro.DOColor(transparentColor, duration / 2)
            .SetLoops(timeLoop, LoopType.Yoyo)
            .SetEase(Ease.InOutSine).OnComplete(() =>
            {
                //Bắt đầu đánh boss
                SpawnEnemies.StartFightBossRightNow(true);
                transform.parent.gameObject.SetActive(false);
            });
    }
    private void PlaySound()
    {
        if(SFX != null)
        {
            SoundFXManager.Instance.PlaySoundFXClip(SFX, transform.parent, false);
        }
    }
}
