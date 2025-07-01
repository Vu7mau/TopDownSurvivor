using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PausePanelTwo : MonoBehaviour
{
    public GameObject pausePanel;
    public GameObject settingsPanel;
    public EffectSignIn effectPausePanel;
    public EffectPanelSetting effectPanelSetting;

    private bool isTransitioning = false;
    private const float transitionDelay = 1f;
    private bool isInSettingPanel = false;
    private void Start()
    {
        settingsPanel.SetActive(false);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !isTransitioning 
            && !isInSettingPanel
            && !DOTween.IsTweening(effectPausePanel.transform))
        {
            if (!pausePanel.activeSelf)
            {
                isTransitioning = true;
                pausePanel.SetActive(true);
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
                    ResumeGame();
                    isTransitioning = false;
                });
            }
        }
    }
    public void ResumeGame()
    {
        Time.timeScale = 1f;
        effectPausePanel.HidePanel(() =>
        {
            isTransitioning = false;
        });
    }
    public void Restart(int sceneIndex)
    {
        Time.timeScale = 1f;
        LevelManager.Instance.LoadLevel(sceneIndex);
    }
    public void BackToMainMenu(int sceneIndex)
    {
        Time.timeScale = 1f;
        LevelManager.Instance.LoadLevel(sceneIndex);
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
}
