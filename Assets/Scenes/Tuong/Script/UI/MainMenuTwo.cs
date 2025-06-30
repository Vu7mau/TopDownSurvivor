using UnityEngine;
using System.Collections;
using static EffectPanelSetting;
public class MainMenuTwo : MonoBehaviour
{
    public static MainMenuTwo Instance;
    AuthManager authManager;
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
    [SerializeField] private EffectSignIn pauseSignInEffect;
    [SerializeField] private GameObject pausePanel;
    public EffectPanelSetting settingEffect;
    [SerializeField] private GameObject settingsPanel;
    private int previousPanelId;
    public GameObject PlayMenu => playMenu;
    public GameObject LogoutButton => logoutButton;
    public GameObject PlayButton => playButton;
    public GameObject SettingPanel => settingsPanel;
    public GameObject PausePanel => pausePanel;
    private void Start()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        authManager = FindObjectOfType<AuthManager>();
        settingsPanel.SetActive(true);
        bool hasLoggedIn = PlayerPrefs.GetInt("HasLoggedIn", 0) == 1;
        logoutButton.SetActive(hasLoggedIn);
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
            else if (previousPanelId == 1)  
            {
                pausePanel.SetActive(true);
                pauseSignInEffect.ShowPanel();
            }
        });
    }
    public void ExitGame()
    {
        UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();
    }
    public void SetLoginState(bool isLoggedIn)
    {
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
    private IEnumerator ClearInputResetPassword(float delay)
    {
        yield return new WaitForSeconds(delay);
        authManager.emailInputField.text = string.Empty;
    }
    public void ExitPanelLogin()
    {
        NotificationUI.Instance.HideImmediately();
        effectLogin.HidePanel(() =>
        {
            PlayMenu.SetActive(true);
            logoutButton.SetActive(PlayerPrefs.GetInt("HasLoggedIn", 0) == 1);
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
            StartCoroutine(ClearInputLogin(0.5f));
        });
    }
    public void CloseResetPasswordPanel()
    {
        NotificationUI.Instance.HideImmediately();
        effectResetPassword.HidePanel(() =>
        {
            PlayMenu.SetActive(true);
            logoutButton.SetActive(PlayerPrefs.GetInt("HasLoggedIn", 0) == 1);
            StartCoroutine(ClearInputResetPassword(0.5f));
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
}
