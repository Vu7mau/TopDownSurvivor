using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class ModePanel : MonoBehaviour
{
    public enum Mode
    {
        Campaign,
        Survive
    }
    [Header("UI Refences")]
    public Button leftButton;
    public Button rightButton;
    public TextMeshProUGUI modeText;
    public TextMeshProUGUI modeDescriptionText;
    public Image backgroundImage;
    public Button playButton;

    public RectTransform modeContentContainer;
    public float transitionDistance = 800f;
    public float transitionDuration = 0.4f;
    public Ease transitionEase = Ease.OutCubic;
    private bool isTransitioning = false;

    [Header("Mode Settings")]
    public Sprite campaignBackground;
    public Sprite surviveBackground;
    [Header("Scene Index")]
    public int campaignSceneIndex = 1;
    public int surviveSceneIndex = 2;
    private Mode currentMode = Mode.Campaign;
    private string campaignDescription = "Khám phá cốt truyện hấp dẫn, chinh phục từng thử thách theo hành trình.";
    private string surviveDescription = "Sinh tồn giữa vùng đất lạ, chiến đấu không ngừng để sống sót càng lâu càng tốt.";
    private void Start()
    {
        leftButton.onClick.AddListener(OnPrevMode);
        rightButton.onClick.AddListener(OnNextMode);
        playButton.onClick.AddListener(OnPlay);
        RefeshUI();
    }
    private void RefeshUI()
    {
        modeText.text = currentMode == Mode.Campaign ? "Chiến dịch" : "Sinh tồn";
        backgroundImage.sprite = currentMode == Mode.Campaign ? campaignBackground : surviveBackground;
        modeDescriptionText.text = currentMode == Mode.Campaign ? campaignDescription : surviveDescription;
    }
    private void OnPrevMode()
    {
        if(isTransitioning) return;
        currentMode = (Mode)(((int)currentMode - 1 + 2) % 2);
        AnimatedModeTransition(-1);
    }
    private void OnNextMode()
    {
        if (isTransitioning) return;
        currentMode = (Mode)(((int)currentMode + 1) % 2);
        AnimatedModeTransition(1);
    }
    private void OnPlay()
    {
        int sceneToLoad = currentMode == Mode.Campaign
            ? campaignSceneIndex
            : surviveSceneIndex;
        LevelManager.Instance.LoadLevel(sceneToLoad);
    }
    private void AnimatedModeTransition(int direction)
    {
        isTransitioning = true;
        float halfSlide = transitionDistance * 0.33f;
        Vector2 targetPos = modeContentContainer.anchoredPosition + Vector2.right * direction * halfSlide;

        modeContentContainer.DOAnchorPos(targetPos, transitionDuration).SetEase(transitionEase).OnComplete(() =>
        {
            RefeshUI();
            modeContentContainer.anchoredPosition = Vector2.right * direction * halfSlide;
            modeContentContainer.DOAnchorPos(Vector2.zero, transitionDuration / 2).SetEase(transitionEase)
            .OnComplete(()=> isTransitioning = false);
        });
    }
}
