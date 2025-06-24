using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class MainMenuTwo : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject loginPanel;
    [SerializeField] private GameObject optionsPanel;
    [SerializeField] private GameObject highScoresPanel;
    [SerializeField] private GameObject creditsPanel;
    [SerializeField] private GameObject newgamePanel;
    [SerializeField] private int sceneIndex = 1;
    [SerializeField] private GameObject loginButton;
    [SerializeField] private GameObject newGameButton;
    [SerializeField] private GameObject continueButton;
    [SerializeField] private GameObject optionsButton;
    [SerializeField] private GameObject highScoreButton;
    private void Start()
    {
        mainMenuPanel.SetActive(true);
        loginPanel.SetActive(false);
        bool hasLoggedIn = PlayerPrefs.GetInt("HasLoggedIn", 0) == 1;
        SetLoginState(hasLoggedIn);
    }
    public void LoginButton()
    {
        mainMenuPanel.SetActive(false);
        loginPanel.SetActive(true);
    }
    public void NewGameButton()
    {
        PlayerPrefs.DeleteKey("SavedLevel");
        StartCoroutine(LoadSceneAsync());
    }
    private IEnumerator LoadSceneAsync()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneIndex);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
    public void OptionsButton()
    {
        optionsPanel.SetActive(true);
    }
    public void ExitOptions()
    {
        optionsPanel.SetActive(false);
    }
    public void HighScoresButton()
    {
        highScoresPanel.SetActive(true);
    }
    public void ExitHighScores()
    {
        highScoresPanel.SetActive(false);
    }
    public void CreditsButton()
    {
        creditsPanel.SetActive(true);
    }
    public void ExitCredits()
    {
        creditsPanel.SetActive(false);
    }
    public void ExitGame()
    {
        UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();
    }
    public void SetLoginState(bool isLoggedIn)
    {
        if (loginButton != null)
        {
            loginButton.SetActive(!isLoggedIn);
        }
        newGameButton.GetComponent<Button>().interactable = isLoggedIn;
        continueButton.GetComponent<Button>().interactable = isLoggedIn;
        optionsButton.GetComponent<Button>().interactable = isLoggedIn;
        highScoreButton.GetComponent<Button>().interactable = isLoggedIn;
    }
    public void ExitPanelSignIn()
    {
        loginPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }
    public void LoadCampaignScene(int index)
    {
        SceneManager.LoadScene(index);
    }
    public void LoadSurvive(int index)
    {
        SceneManager.LoadScene(index);
    }
    public void BackToMainMenu()
    {
        newgamePanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }
    public void OpenNewGamePanel()
    {
        newgamePanel.SetActive(true);
    }
}
