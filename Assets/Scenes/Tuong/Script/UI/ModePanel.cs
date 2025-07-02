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
        if (isTransitioning) return;
        isTransitioning = true;
        modeText.text = currentMode == Mode.Campaign ? "Chiến dịch" : "Sinh tồn";
        modeDescriptionText.text = currentMode == Mode.Campaign
            ? campaignDescription
            : surviveDescription;
        Sprite newSprite = currentMode == Mode.Campaign
            ? campaignBackground
            : surviveBackground;

        var parent = backgroundImage.transform.parent;
        GameObject cloneGO = Instantiate(backgroundImage.gameObject, parent);
        var cloneImg = cloneGO.GetComponent<Image>();

        cloneImg.sprite = newSprite;
        RectTransform rtOrig = backgroundImage.rectTransform;
        RectTransform rtClone = cloneImg.rectTransform;

        Vector2 origPos = rtOrig.anchoredPosition;
        Vector2 offPosOld = origPos + Vector2.right * direction * transitionDistance;
        Vector2 offPosNew = origPos - Vector2.right * direction * transitionDistance;
        rtClone.anchoredPosition = offPosNew;
        Sequence seq = DOTween.Sequence();

        seq.Join(rtOrig
            .DOAnchorPos(offPosOld, transitionDuration)
            .SetEase(transitionEase)
        );
        seq.Join(rtClone
            .DOAnchorPos(origPos, transitionDuration)
            .SetEase(transitionEase)
        );
        seq.OnComplete(() =>
        {
            backgroundImage.sprite = newSprite;
            rtOrig.anchoredPosition = origPos;
            Destroy(cloneGO);
            isTransitioning = false;
        });
    }
}
