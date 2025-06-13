using PlayFab;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class AuthManager : TuongMonobehaviour
{
    [Header("Sign Up")]
    [SerializeField] protected TMP_InputField signUpUserName;
    private string getSignUpUserName = "SignUpUserName";
    [SerializeField] protected TMP_InputField signUpEmail;
    private string getSignUpEmail = "SignUpEmail";
    [SerializeField] protected TMP_InputField signUpPassword;
    private string getSignUpPassword = "SignUpPassword";
    [SerializeField] protected TMP_InputField otp;
    private string getOtp = "OTPInputField";
    [Header("Sign In")]
    [SerializeField] protected TMP_InputField signInEmail;
    private string getSignInEmail = "SignInEmail";
    [SerializeField] protected TMP_InputField signInPassword;
    private string getSignInPassword = "SignInPassword";
    [Header("Panel")]
    [SerializeField] protected GameObject signUpPanel;
    private string getSignUpPanel = "SignUpPanel";
    [SerializeField] protected GameObject signInPanel;
    private string getSignInPanel = "SignInPanel";
    [SerializeField] protected GameObject otpPanel;
    private string getOTPPanel = "OTPPanel";
    [SerializeField] protected GameObject resetPasswordPanel;
    private string getResetPasswordPanel = "ResertPasswordPanel";
    [SerializeField] protected GameObject buttonLogin;
    private string getButtonLogin = "ButtonLogin";
    [Header("Reset Password")]
    [SerializeField] protected TMP_InputField emailInputField;
    private string getEmailInputField = "ResetPasswordEmail";
    [SerializeField] protected TextMeshProUGUI message;
    private string getMessage = "Message";
    [SerializeField] protected Button backButton;
    private string getBackButton = "BackToSignInButton";
    [Header("LoadScene")]
    public int levelIndex = 1;
    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadTMPInputField();
        this.LoadGameObject();
        this.LoadButton();
        this.LoadTextMeshProUGUI();
    }
    protected virtual void LoadTMPInputField()
    {
        if (signUpUserName == null) signUpUserName = LoadTMPInputField(signUpUserName, getSignUpUserName);
        if (signUpEmail == null) signUpEmail = LoadTMPInputField(signUpEmail, getSignUpEmail);
        if (signUpPassword == null) signUpPassword = LoadTMPInputField(signUpPassword, getSignUpPassword);
        if (otp == null) otp = LoadTMPInputField(otp, getOtp);
        if (signInEmail == null) signInEmail = LoadTMPInputField(signInEmail, getSignInEmail);
        if (signInPassword == null) signInPassword = LoadTMPInputField(signInPassword, getSignInPassword);
        if (emailInputField == null) emailInputField = LoadTMPInputField(emailInputField, getEmailInputField);
    }
    protected virtual void LoadGameObject()
    {
        if (signUpPanel == null) signUpPanel = LoadGameObject(signUpPanel, getSignUpPanel);
        if (signInPanel == null) signInPanel = LoadGameObject(signInPanel, getSignInPanel);
        if (otpPanel == null) otpPanel = LoadGameObject(otpPanel, getOTPPanel);
        if (buttonLogin == null) buttonLogin = LoadGameObject(buttonLogin, getButtonLogin);
        if (resetPasswordPanel == null) resetPasswordPanel = LoadGameObject(resetPasswordPanel, getResetPasswordPanel);
    }
    protected virtual void LoadButton()
    {
        if (backButton == null) backButton = LoadButton(backButton, getBackButton);
    }
    protected virtual void LoadTextMeshProUGUI()
    {
        if(message == null) message = LoadTextMeshProUGUI(message, getMessage);
    }
    public void OpenSignUpPanel()
    {
        signUpPanel.SetActive(true);
        signInPanel.SetActive(false);
    }
    public void OpenSignInPanel()
    {
        signUpPanel.SetActive(false);
        signInPanel.SetActive(true);
    }
    public void ButtonLogin()
    {
        signUpPanel.SetActive(true);
        buttonLogin.SetActive(false);
    }
    public void BackToSignIn()
    {
        resetPasswordPanel.SetActive(false);
        signInPanel.SetActive(true);
    }
    public void ForgotYourPassword()
    {
        signInPanel.SetActive(false);
        resetPasswordPanel.SetActive(true);
    }
    public void ExitPanelOTP()
    {
        otpPanel.SetActive(false);
        signUpPanel.SetActive(true);
    }
    public void LogOut()
    {
        PlayFabClientAPI.ForgetAllCredentials();
        PlayerPrefs.SetInt("HasLoggedIn", 0);
        PlayerPrefs.SetInt("AutoLoginDisable", 1);      
        PlayerPrefs.Save();
        MainMenuTwo mainMenuTwo = FindObjectOfType<MainMenuTwo>();
        if (mainMenuTwo != null) mainMenuTwo.SetLoginState(false);
        Debug.Log("Đăng xuất thành công.");
    }
}
