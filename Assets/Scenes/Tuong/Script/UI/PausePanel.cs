using UnityEngine;
using UnityEngine.SceneManagement;
public class PausePanel : MonoBehaviour
{
    public GameObject pausePanel;
    public GameObject settingsPanel;
    public EffectSignIn effectPausePanel;
    private void Start()
    {
        settingsPanel.SetActive(false);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!MainMenuTwo.Instance.PausePanel.activeSelf)
            {
                MainMenuTwo.Instance.PlayMenu.SetActive(false);
                MainMenuTwo.Instance.PausePanel.SetActive(true);
                effectPausePanel.ShowPanel();
            }
            else
            {
                effectPausePanel.HidePanel(() =>
                {
                    MainMenuTwo.Instance.PausePanel.SetActive(false);
                    MainMenuTwo.Instance.PlayMenu.SetActive(true);
                    ResumeGame();

                });
            }
        }
    }
    public void ResumeGame()
    {
        Time.timeScale = 1f;
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
        effectPausePanel.HidePanel(() =>
        {
            pausePanel.SetActive(false);
            MainMenuTwo.Instance.OpenSettingFromPause();
            settingsPanel.SetActive(true);
        });
    }
    public void CloseSettings()
    {
        MainMenuTwo.Instance.CloseSetingPanel();
    }
}
