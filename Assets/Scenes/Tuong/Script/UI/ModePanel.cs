using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
    public Image backgroundImage;
    public Button playButton;

    [Header("Mode Settings")]
    public Sprite campaignBackground;
    public Sprite surviveBackground;
    [Header("Scene Index")]
    public int campaignSceneIndex = 1;
    public int surviveSceneIndex = 2;
    private Mode currentMode = Mode.Campaign;
    private void Start()
    {
        leftButton.onClick.AddListener(OnPrevMode);
        rightButton.onClick.AddListener(OnNextMode);
        playButton.onClick.AddListener(OnPlay);
        RefreshUI();
    }
    private void OnPrevMode()
    {
        currentMode = (Mode)(((int)currentMode -1 + 2) % 2);
        RefreshUI();
    }
    private void OnNextMode()
    {
        currentMode = (Mode)(((int)currentMode + 1) % 2);
        RefreshUI();
    }
    private void RefreshUI()
    {
        modeText.text = currentMode == Mode.Campaign ? "Chiến dịch" : "Sinh tồn";
        backgroundImage.sprite = currentMode == Mode.Campaign 
            ? campaignBackground 
            : surviveBackground;
    }
    private void OnPlay()
    {
        int sceneToLoad = currentMode == Mode.Campaign
            ? campaignSceneIndex
            : surviveSceneIndex;
        LevelManager.Instance.LoadLevel(sceneToLoad);
    }
}
