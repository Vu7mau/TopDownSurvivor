using UnityEngine;
using System.Collections;
using PlayFab;
public class MainMenuTwo : MonoBehaviour
{
    public static MainMenuTwo Instance;
    public AuthManager authManager;
    [SerializeField] private EffectSignIn effectLogin;
    [SerializeField] private EffectSignIn effectRegister;
    [SerializeField] private EffectSignIn effectResetPassword;
    [SerializeField] private GameObject loginPanel;
    [SerializeField] private GameObject registerPanel;
    [SerializeField] private GameObject resetPanel;
    [SerializeField] private GameObject playButton;
    [SerializeField] private GameObject settingButton;
    [SerializeField] private GameObject logoutButton;
    [SerializeField] private GameObject playMenu;
    [SerializeField] private GameObject modePanel;
    [SerializeField] private GameObject leaderboardPanel;
    [SerializeField] private GameObject iconLeaderboard;
    [SerializeField] private GameObject instructPanel;
    public EffectPanelSetting settingEffect;
    [SerializeField] private GameObject settingsPanel;
    private int previousPanelId;
    public GameObject PlayMenu => playMenu;
    public GameObject LogoutButton => logoutButton;
    public GameObject PlayButton => playButton;
    public GameObject SettingPanel => settingsPanel;
    public GameObject ResetPanel => resetPanel;
    public EffectSignIn EffectLogin => effectLogin;
    public EffectSignIn EffectResetPassword => effectResetPassword;
    public GameObject LoginPanel => loginPanel;
    public GameObject ModePanelPublic => modePanel;
    public GameObject IconLeaderBoard => iconLeaderboard;
    private void Start()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        authManager = FindObjectOfType<AuthManager>();
        settingsPanel.SetActive(true);
        bool hasLoggedIn = PlayerPrefs.GetInt("HasLoggedIn", 0) == 1;
        logoutButton.SetActive(hasLoggedIn);
        iconLeaderboard.SetActive(hasLoggedIn);
        HidePanel();
    }
    public void HidePanel()
    {
        playMenu.SetActive(true);
        loginPanel.SetActive(false);
        registerPanel.SetActive(false);
        resetPanel.SetActive(false);
        settingsPanel.SetActive(false);
        modePanel.SetActive(false);
        leaderboardPanel.SetActive(false);
        instructPanel.SetActive(false);
    }
    public void OpenSettingFromMainMenu()
    {
        previousPanelId = 0;
        ShowSettings();
    }
    public void OpenSettingFromPause()
    {
        previousPanelId = 1;
        ShowSettings();
    }
    public void ShowSettings()
    {
        if(previousPanelId == 0)
            playMenu.SetActive(false);
        settingsPanel.SetActive(true);
        settingEffect.ShowPanel();
    }
    public void CloseSetingPanel()
    {
        settingEffect.HidePanel(() =>
        {
            settingsPanel.SetActive(false);
            if(previousPanelId == 0)
            {
                playMenu.SetActive(true);
            }
        });
    }
    public void ExitGame()
    {
        UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();
    }
    private IEnumerator ClearInputLogin(float delay)
    {
        yield return new WaitForSeconds(delay);
        authManager.signInEmail.text = string.Empty;
        authManager.signInPassword.text = string.Empty;
    }
    private IEnumerator ClearInputRegister(float delay)
    {
        yield return new WaitForSeconds(delay);
        authManager.signUpUserName.text = string.Empty;
        authManager.signUpEmail.text = string.Empty;
        authManager.signUpPassword.text = string.Empty;
    }
    public IEnumerator ClearInputResetPassword(float delay)
    {
        yield return new WaitForSeconds(delay);
        authManager.resetPasswordInput.text = string.Empty;
        authManager.signInEmail.text = string.Empty;
        authManager.signInPassword.text = string.Empty;
    }
    public void ExitPanelLogin()
    {
        NotificationUI.Instance.HideImmediately();
        effectLogin.HidePanel(() =>
        {
            PlayMenu.SetActive(true);
            logoutButton.SetActive(PlayerPrefs.GetInt("HasLoggedIn", 0) == 1);
            iconLeaderboard.SetActive(PlayerPrefs.GetInt("HasLoggedIn", 0) == 1);
            StartCoroutine(ClearInputLogin(0.5f));
        });
    }
    public void OpenOptionsPanel()
    {
        settingsPanel.SetActive(true);
    }
    public void CloseOptionsPanel()
    {
        settingsPanel.SetActive(false);
    }
    public void Play()
    {
        bool hasLoggedIn = PlayerPrefs.GetInt("HasLoggedIn", 0) == 1;
        if (hasLoggedIn)
        {
            modePanel.SetActive(true);
            ModePanel.Instance.SetUpPanel();
        }
        else
        {
            loginPanel.SetActive(true);
        }
        playMenu.SetActive(false);
    }
    public void ExitPanelRegister()
    {
        NotificationUI.Instance.HideImmediately();
        effectRegister.HidePanel(() =>
        {
            PlayMenu.SetActive(true);
            logoutButton.SetActive(PlayerPrefs.GetInt("HasLoggedIn", 0) == 1);
            iconLeaderboard.SetActive(PlayerPrefs.GetInt("HasLoggedIn", 0) == 1);
            StartCoroutine(ClearInputRegister(0.5f));
        });
    }
    public void OpenSignUpPanel()
    {
        effectLogin.HidePanel(() =>
        {
            loginPanel.SetActive(false);
            registerPanel.SetActive(true);
            effectRegister.ShowPanel();
            StartCoroutine(ClearInputLogin(0.5f));
        });
    }
    public void OpenResetPasswordPanel()
    {
        effectLogin.HidePanel(() =>
        {
            loginPanel.SetActive(false);
            resetPanel.SetActive(true);
            effectResetPassword.ShowPanel();
            StartCoroutine(ClearInputResetPassword(0.01f));
        });
    }
    private IEnumerator DelayedClear()
    {
        yield return null;                // hoặc yield return new WaitForSeconds(0.1f);
        authManager.resetPasswordInput.text = string.Empty;
    }
    public void CloseResetPasswordPanel()
    {
        NotificationUI.Instance.HideImmediately();
        effectResetPassword.HidePanel(() =>
        {
            authManager.resetPasswordInput.text = string.Empty;
            PlayMenu.SetActive(true);
            logoutButton.SetActive(PlayerPrefs.GetInt("HasLoggedIn", 0) == 1);
            iconLeaderboard.SetActive(PlayerPrefs.GetInt("HasLoggedIn", 0) == 1);
        });
    }
    public void OpenPanelLogin()
    {
        effectRegister.HidePanel(() =>
        {
            registerPanel.SetActive(false);
            loginPanel.SetActive(true);
            effectLogin.ShowPanel();
            StartCoroutine(ClearInputRegister(0.5f));
        });
    }
    public void CloseSettingPanel()
    {
        settingEffect.HidePanel(() =>
        {
            settingsPanel.SetActive(false);
            playMenu.SetActive(true);
        });
    }
    public void OpenLeaderBoardPanel()
    {
        playMenu.SetActive(false);
        leaderboardPanel.SetActive(true);
        iconLeaderboard.SetActive(false);
        StartCoroutine(LoadLeaderboardAfterUIReady());
    }
    public void CloseLeaderBoardPanel()
    {
        LeaderBoardManager.Instance.scrollRectCampign.verticalNormalizedPosition = 1f;
        LeaderBoardManager.Instance.scrollRectSurvive.verticalNormalizedPosition = 1f;
        iconLeaderboard.SetActive(true);
        playMenu.SetActive(true);
        leaderboardPanel.SetActive(false);
    }
    private IEnumerator LoadLeaderboardAfterUIReady()
    {
        yield return new WaitUntil(() => PlayFabClientAPI.IsClientLoggedIn());
        LeaderBoardCampaign.Instance?.GetLeaderBoardCampaign();
        LeaderBoardSurvive.Instance?.GetLeaderBoardSurvive();
        LeaderBoardCampaign.Instance?.GetMyRank();
        LeaderBoardSurvive.Instance?.GetMyRank();
    }
    public void OpenInstructPanel()
    {
        playMenu.SetActive(false);
        instructPanel.SetActive(true);
    }
    public void CloseInstructPanel()
    {
        instructPanel.SetActive(false);
        playMenu.SetActive(true);
    }
}
