using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ModePanel : MonoBehaviour
{
    public static ModePanel Instance;
    public enum Mode { Campaign, Survive }

    [Header("UI References")]
    public Button leftButton;
    public Button rightButton;
    public TextMeshProUGUI modeText;
    public TextMeshProUGUI modeDescriptionText;
    public Button playButton;
    public RectTransform modeContentContainer;

    [Header("Backgrounds")]
    public Image backgroundImageA;
    public Image backgroundImageB;

    [Header("LockUI")]
    public GameObject lockOverlay;
    public Button unlockButton;
    public const string SURVIVE_UNLOCK_KEY = "SurviveUnlocked";

    public float transitionDistance = 800f;
    public float transitionDuration = 0.4f;
    public Ease transitionEase = Ease.OutCubic;

    private bool isTransitioning = false;
    private bool usingA = true;

    [Header("Mode Settings")]
    public Sprite campaignBackground;
    public Sprite surviveBackground;

    [Header("Scene Index")]
    public int campaignSceneIndex = 1;
    public int surviveSceneIndex = 2;
    private Sequence transitionSequence;
    private bool isSceneLoading;

    private Mode currentMode = Mode.Campaign;
    private string campaignDescription = "Khám phá cốt truyện hấp dẫn, chinh phục từng thử thách theo hành trình.";
    private string surviveDescription = "Sinh tồn giữa vùng đất lạ, chiến đấu không ngừng để sống sót càng lâu càng tốt.";

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
#if UNITY_EDITOR
        resetUnlockButton.GetComponent<Button>().onClick.AddListener(ResetSurviveUnlock);
        resetUnlockButton.SetActive(true);
#endif
    }

    private void OnEnable()
    {
        currentMode = Mode.Campaign;
        ResetBackgroundState();
        RefreshUIInstant();
    }

    public void RefreshUIInstant()
    {
        modeText.text = currentMode == Mode.Campaign ? "Chiến dịch" : "Sinh tồn";
        modeDescriptionText.text = currentMode == Mode.Campaign ? campaignDescription : surviveDescription;
        (usingA ? backgroundImageA : backgroundImageB).sprite = currentMode == Mode.Campaign ? campaignBackground : surviveBackground;
        UpdateLockStateUI();
    }

    private void ResetBackgroundState()
    {
        backgroundImageA.rectTransform.anchoredPosition = Vector2.zero;
        backgroundImageB.rectTransform.anchoredPosition = Vector2.zero;
        usingA = true;
        backgroundImageA.gameObject.SetActive(true);
        backgroundImageB.gameObject.SetActive(false);
        backgroundImageA.sprite = currentMode == Mode.Campaign ? campaignBackground : surviveBackground;
    }

    public void OnPrevMode() { if (!isTransitioning) AnimateTransition(-1); }
    public void OnNextMode() { if (!isTransitioning) AnimateTransition(1); }

    public void OnPlay()
    {
        transitionSequence?.Kill();
        DOTween.Kill(gameObject, true);
        LevelManager.Instance.LoadLevel(currentMode == Mode.Campaign ? campaignSceneIndex : surviveSceneIndex);
    }

    private void AnimateTransition(int direction)
    {
        isTransitioning = true;
        currentMode = (Mode)(((int)currentMode + (direction > 0 ? 1 : -1) + 2) % 2);
        modeText.text = currentMode == Mode.Campaign ? "Chiến dịch" : "Sinh tồn";
        modeDescriptionText.text = currentMode == Mode.Campaign ? campaignDescription : surviveDescription;

        Image from = usingA ? backgroundImageA : backgroundImageB;
        Image to = usingA ? backgroundImageB : backgroundImageA;
        usingA = !usingA;
        to.sprite = currentMode == Mode.Campaign ? campaignBackground : surviveBackground;

        var rtFrom = from.rectTransform;
        var rtTo = to.rectTransform;
        Vector2 origin = rtFrom.anchoredPosition;
        Vector2 outPos = origin + Vector2.right * direction * transitionDistance;
        Vector2 inPos = origin - Vector2.right * direction * transitionDistance;

        rtTo.anchoredPosition = inPos;
        to.gameObject.SetActive(true);

        transitionSequence = DOTween.Sequence()
            .Join(rtFrom.DOAnchorPos(outPos, transitionDuration).SetEase(transitionEase))
            .Join(rtTo.DOAnchorPos(origin, transitionDuration).SetEase(transitionEase))
            .OnComplete(() => {
                rtFrom.anchoredPosition = origin;
                rtFrom.gameObject.SetActive(false);
                UpdateLockStateUI();
                isTransitioning = false;
            });
    }

    private bool IsSurviveUnlocked() => PlayerPrefs.GetInt(SURVIVE_UNLOCK_KEY, 0) == 1;

    private void UpdateLockStateUI()
    {
        if (isSceneLoading) return;
        if (currentMode == Mode.Survive && !IsSurviveUnlocked())
        {
            lockOverlay.SetActive(true);
            playButton.interactable = false;
        }
    }

    public void UnlockSurviveMode()
    {
        PlayerPrefs.SetInt(SURVIVE_UNLOCK_KEY, 1);
        PlayerPrefs.Save();
    }

    public void ResetSurviveUnlock()
    {
        PlayerPrefs.SetInt(SURVIVE_UNLOCK_KEY, 0);
        PlayerPrefs.Save();
        Debug.Log("❌ SURVIVE_UNLOCK_KEY reset về 0");
        if (!isTransitioning)
            RefreshUIInstant();
    }
    public void Back()
    {
        isSceneLoading = false;
        MainMenuTwo.Instance.ModePanelPublic.SetActive(false);
        MainMenuTwo.Instance.PlayMenu.SetActive(true);
        MainMenuTwo.Instance.LoginPanel.SetActive(false);
    }
#if UNITY_EDITOR
    public GameObject resetUnlockButton;
#endif
}
