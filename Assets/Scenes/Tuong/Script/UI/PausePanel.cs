using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PausePanel : MonoBehaviour
{
    public GameObject pausePanel;
    public GameObject settingsPanel;
    public EffectSignIn effectPausePanel;
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
            if (!MainMenuTwo.Instance.PausePanel.activeSelf)
            {
                isTransitioning = true;
                MainMenuTwo.Instance.PlayMenu.SetActive(false);
                MainMenuTwo.Instance.PausePanel.SetActive(true);
                effectPausePanel.ShowPanel();
                DOVirtual.DelayedCall(transitionDelay, () =>
                {
                    isTransitioning = false;
                });
            }
            else
            {
                isTransitioning = true;
                effectPausePanel.HidePanel(() =>
                {
                    MainMenuTwo.Instance.PausePanel.SetActive(false);
                    MainMenuTwo.Instance.PlayMenu.SetActive(true);
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
            MainMenuTwo.Instance.PausePanel.SetActive(false);
            MainMenuTwo.Instance.PlayMenu.SetActive(true);
            isTransitioning = false;
        });
    }
    public void Restart(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }
    public void BackToMainMenu(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }
    public void Settings()
    {
        isTransitioning = true;
        isInSettingPanel = true;
        effectPausePanel.HidePanel(() =>
        {
            pausePanel.SetActive(false);
            MainMenuTwo.Instance.OpenSettingFromPause();
            settingsPanel.SetActive(true);
            isTransitioning = false;
        });
    }
    public void CloseSettings()
    {
        isTransitioning = false;
        isInSettingPanel = false;
        MainMenuTwo.Instance.CloseSetingPanel();
    }
}
