using PlayFab;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class AuthManager : TuongMonobehaviour
{
    [Header("Sign Up")]
    public TMP_InputField signUpUserName;
    private string getSignUpUserName = "SignUpUserName";
    public TMP_InputField signUpEmail;
    private string getSignUpEmail = "SignUpEmail";
    public TMP_InputField signUpPassword;
    private string getSignUpPassword = "SignUpPassword";
    public TMP_InputField otp;
    private string getOtp = "OTPInputField";
    [Header("Sign In")]
    public TMP_InputField signInEmail;
    private string getSignInEmail = "SignInEmail";
    public TMP_InputField signInPassword;
    private string getSignInPassword = "SignInPassword";
    [Header("Panel")]
    [SerializeField] protected GameObject signUpPanel;
    private string getSignUpPanel = "SignUpPanel";
    public GameObject signInPanel;
    private string getSignInPanel = "SignInPanel";
    public GameObject otpPanel;
    private string getOTPPanel = "OTPPanel";
    [SerializeField] protected GameObject resetPasswordPanel;
    private string getResetPasswordPanel = "ResertPasswordPanel";
    [SerializeField] protected GameObject buttonLogin;
    private string getButtonLogin = "ButtonLogin";
    [Header("Reset Password")]
    public TMP_InputField emailInputField;
    private string getEmailInputField = "ResetPasswordEmail";
    [SerializeField] protected TextMeshProUGUI message;
    private string getMessage = "Message";
    [Header("LoadScene")]
    public int levelIndex = 1;
    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadTMPInputField();
        this.LoadGameObject();
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
    protected virtual void LoadTextMeshProUGUI()
    {
        if (message == null) message = LoadTextMeshProUGUI(message, getMessage);
    }
    public void LogOut()
    {
        CharacterInformation.Instance.ClearCharacterInfo();
        PlayFabClientAPI.ForgetAllCredentials();
        PlayerPrefs.SetInt("HasLoggedIn", 0);
        PlayerPrefs.SetInt("AutoLoginDisable", 1);
        PlayerPrefs.Save();
        MainMenuTwo.Instance.PlayMenu.SetActive(true);
        MainMenuTwo.Instance.LogoutButton.SetActive(false);
        Debug.Log("Đăng xuất thành công.");
    }
}
