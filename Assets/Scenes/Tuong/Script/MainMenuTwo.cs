using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenuTwo : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject loginPanel;
    [SerializeField] private GameObject optionsPanel;
    [SerializeField] private GameObject highScoresPanel;
    [SerializeField] private GameObject creditsPanel;
    [SerializeField] private int sceneIndex = 1;
    private void Start()
    {
        mainMenuPanel.SetActive(true);
        loginPanel.SetActive(false);
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
}
