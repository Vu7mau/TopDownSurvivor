using DG.Tweening;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PausePanelTwo : MonoBehaviour
{
    public static PausePanelTwo Instance;
    public GameObject pausePanel;
    public GameObject backGround;
    public GameObject settingsPanel;
    public EffectSignIn effectPausePanel;
    public EffectPanelSetting effectPanelSetting;
    public GameObject timerPrefab;

    [SerializeField] protected Transform winPanel;
    [SerializeField] protected Transform losePanel;

    private bool isTransitioning = false;
    private const float transitionDelay = 1f;
    private bool isInSettingPanel = false;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        if (CountDownTimer.Instance == null)
        {
            Instantiate(timerPrefab);
        }
        settingsPanel.SetActive(false);
        pausePanel.SetActive(false);
        backGround.SetActive(false);

    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)
            && !isTransitioning
            && !isInSettingPanel
            && !DOTween.IsTweening(effectPausePanel.transform)
            && !IsWinOrLosePanelActive()) 
        {
            if (!pausePanel.activeSelf)
            {
                isTransitioning = true;
                pausePanel.SetActive(true);
                backGround.SetActive(true);
                effectPausePanel.ShowPanel();
                DOVirtual.DelayedCall(0.01f, () =>
                {
                    Time.timeScale = 0f;
                }).SetUpdate(true);
                DOVirtual.DelayedCall(transitionDelay, () =>
                {
                    isTransitioning = false;
                }).SetUpdate(true);
            }
            else
            {
                isTransitioning = true;
                effectPausePanel.HidePanel(() =>
                {
                    backGround.SetActive(false);
                    ResumeGame();
                    isTransitioning = false;
                });
            }
        }
    }

    private bool IsWinOrLosePanelActive()
    {
        return (winPanel != null && winPanel.gameObject.activeSelf)
            || (losePanel != null && losePanel.gameObject.activeSelf);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        effectPausePanel.HidePanel(() =>
        {
            backGround.SetActive(false);
            isTransitioning = false;
        });
    }

    public async void RestartTheGame()
    {
        Time.timeScale = 1f;
        DOTween.Kill(gameObject);
        
        winPanel.gameObject.SetActive(false);
        losePanel.gameObject.SetActive(false);
        int finalScore = PlayerPrefs.GetInt("currentScores", 0);
        int coins = PlayerPrefs.GetInt("currentCoins", 0);
        PlayerScoreManager.Instance?.SendFinalScore(finalScore + coins);
        PlayerScoreManager.Instance?.ResetScore();
        CountDownTimer.Instance.ResetTimer();
        await LevelManager.Instance.LoadLevelAsync(SceneManager.GetActiveScene().buildIndex);
    }
    //public async void Restart(int sceneIndex)
    //{
    //    Time.timeScale = 1f;
    //    int finalScore = PlayerPrefs.GetInt("currentScores", 0);
    //    int coins = PlayerPrefs.GetInt("currentCoins", 0);
    //    PlayerScoreManager.Instance?.SendFinalScore(finalScore + coins);
    //    PlayerScoreManager.Instance?.ResetScore();
    //    pausePanel.SetActive(false);
    //    DOTween.KillAll();
    //    await LevelManager.Instance.LoadLevelAsync(sceneIndex);
    //}

    public virtual void BackToMainMenuGame()
    {
        winPanel.gameObject.SetActive(false);
        losePanel.gameObject.SetActive(false);
        this.BackToMainMenu(1);
    }
    public async void BackToMainMenu(int sceneIndex)
    {
        Time.timeScale = 1f;
        int finalScore = PlayerPrefs.GetInt("currentScores", 0);
        int coins = PlayerPrefs.GetInt("currentCoins", 0);
        PlayerScoreManager.Instance?.SendFinalScore(finalScore + coins);
        PlayerScoreManager.Instance?.ResetScore();
        Debug.Log(finalScore + coins);
        CountDownTimer.Instance?.ResetTimer();
        CountDownTimer.Instance?.PauseTimer();
        pausePanel?.SetActive(false);
        losePanel?.gameObject.SetActive(false);
        DOTween.KillAll();
        await LevelManager.Instance.LoadLevelAsync(sceneIndex);
    }
    public void ExitGame()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
    public void Settings()
    {
        isTransitioning = true;
        isInSettingPanel = true;
        effectPausePanel.HidePanel(() =>
        {
            pausePanel.SetActive(false);
            settingsPanel.SetActive(true);
            isTransitioning = false;
        });
    }
    public void CloseSettings()
    {
        isTransitioning = true;
        isInSettingPanel = false;
        effectPanelSetting.HidePanel(() =>
        {
            settingsPanel.SetActive(false);
            pausePanel.SetActive(true);
            effectPausePanel.ShowPanel();
            isTransitioning = false;
        });
    }
    public void ClosePause()
    {
        isTransitioning = true;
        isInSettingPanel = false;
        effectPausePanel.HidePanel(() =>
        {
            pausePanel.SetActive(false);
            backGround.SetActive(false);
            isTransitioning = false;
        });
    }
}
