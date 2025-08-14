using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;

public class SplashScreenController : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject panel;
    [SerializeField] private Image logoImage;
    [SerializeField] private Image titleImage;
    [SerializeField] private GameObject pressText;
    [SerializeField] private float fadeDuration = 1.5f;
    [SerializeField] private float displayTime = 3f;

    [Header("Text Animation")]
    [SerializeField] private float textFallDistance = 300f;
    [SerializeField] private float textFallDuration = 0.5f;
    [SerializeField] private float textBounceAmount = 30f;
    [SerializeField] private float textBounceDuration = 0.15f;
    [SerializeField] private float textFlyUpDistance = 300f;
    [SerializeField] private float textFlyUpDuration = 0.5f;

    private Vector3 textTargetPos;
    private bool canSkip = false; 

    private void Start()
    {
        panel.SetActive(true);

        Color logoColor = logoImage.color;
        logoColor.a = 0f;
        logoImage.color = logoColor;

        textTargetPos = titleImage.transform.localPosition;
        titleImage.transform.localPosition = textTargetPos + Vector3.up * textFallDistance;
        titleImage.gameObject.SetActive(true);

        if (pressText != null)
            pressText.SetActive(false);

        StartCoroutine(StartPlaySequence());
    }

    private void Update()
    {
        if (canSkip && Input.GetMouseButtonDown(0)) 
        {
            panel.gameObject.SetActive(false);
        }
    }

    private IEnumerator StartPlaySequence()
    {
        Tween fallTween = titleImage.transform.DOLocalMoveY(textTargetPos.y, textFallDuration)
            .SetEase(Ease.InExpo);
        yield return fallTween.WaitForCompletion();

        Tween bounceTween = titleImage.transform.DOLocalMoveY(textTargetPos.y + textBounceAmount, textBounceDuration)
            .SetLoops(2, LoopType.Yoyo)
            .SetEase(Ease.OutQuad);
        yield return bounceTween.WaitForCompletion();

        Sequence seq = DOTween.Sequence();
        seq.Append(logoImage.DOFade(1f, fadeDuration).SetEase(Ease.OutSine));
        seq.AppendInterval(displayTime);
        seq.Append(logoImage.DOFade(0f, fadeDuration).SetEase(Ease.InSine));
        yield return seq.WaitForCompletion();

        logoImage.gameObject.SetActive(false);

        Tween flyUpTween = titleImage.transform.DOLocalMoveY(textTargetPos.y + textFlyUpDistance, textFlyUpDuration)
            .SetEase(Ease.InQuad);
        yield return flyUpTween.WaitForCompletion();

        titleImage.gameObject.SetActive(false);

        yield return new WaitForSeconds(2f);

        if (pressText != null)
        {
            pressText.SetActive(true);
            PressToContinueEffect.Instance.PlayEffect();
        }

        canSkip = true; 
    }
}
