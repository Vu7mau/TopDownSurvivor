using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;
public class SplashScreenController : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject panel;
    [SerializeField] private GameObject backgroundImage;
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
    private bool hasSkipped = false;

    private void Start()
    {
        panel.SetActive(true);

        InitIntroObjects();
        StartCoroutine(StartPlaySequence());
    }
    private IEnumerator ShowMenuNextFrame()
    {
        yield return null; 
        if (MainMenuTwo.Instance != null)
        {
            MainMenuTwo.Instance.PlayMenu.SetActive(true);
            MainMenuTwo.Instance.IconLeaderBoard.SetActive(true);
            MainMenuTwo.Instance.IconGame.SetActive(true);
        }
    }
    private void InitIntroObjects()
    {
        logoImage.gameObject.SetActive(true);
        titleImage.gameObject.SetActive(true);
        pressText.gameObject.SetActive(false);

        Color logoColor = logoImage.color;
        logoColor.a = 0f;
        logoImage.color = logoColor;

        textTargetPos = titleImage.transform.localPosition;
        titleImage.transform.localPosition = textTargetPos + Vector3.up * textFallDistance;
    }

    private async void Update()
    {
        if (canSkip && !hasSkipped && Input.GetMouseButtonDown(0))
        {
            hasSkipped = true;
            DOTween.Kill(titleImage.transform);
            DOTween.Kill(logoImage);
            DOTween.Kill(pressText?.transform);

            panel.SetActive(false);
            await LevelManager.Instance.LoadLevelAsync(1);
        }
    }
    private IEnumerator StartPlaySequence()
    {
        yield return titleImage.transform.DOLocalMoveY(textTargetPos.y, textFallDuration)
            .SetEase(Ease.InExpo)
            .WaitForCompletion();

        yield return titleImage.transform.DOLocalMoveY(textTargetPos.y + textBounceAmount, textBounceDuration)
            .SetLoops(2, LoopType.Yoyo)
            .SetEase(Ease.OutQuad)
            .WaitForCompletion();

        Sequence seq = DOTween.Sequence();
        seq.Append(logoImage.DOFade(1f, fadeDuration).SetEase(Ease.OutSine));
        seq.AppendInterval(displayTime);
        seq.Append(logoImage.DOFade(0f, fadeDuration).SetEase(Ease.InSine));
        yield return seq.WaitForCompletion();
        logoImage.gameObject.SetActive(false);

        yield return titleImage.transform.DOLocalMoveY(textTargetPos.y + textFlyUpDistance, textFlyUpDuration)
            .SetEase(Ease.InQuad)
            .WaitForCompletion();
        titleImage.gameObject.SetActive(false);

        backgroundImage.SetActive(false);
        yield return new WaitForSeconds(1f);

        pressText.SetActive(true);
        PressToContinueEffect.Instance.PlayEffect();

        canSkip = true;
    }
}
