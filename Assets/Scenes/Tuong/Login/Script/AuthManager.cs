using TMPro;
using UnityEngine;
public class AuthManager : TuongMonobehaviour
{
    [Header("Sign Up")]
    [SerializeField] protected TMP_InputField signUpUserName;
    private string getSignUpUserName = "SignUpUserName";
    [SerializeField] protected TMP_InputField signUpEmail;
    private string getSignUpEmail = "SignUpEmail";
    [SerializeField] protected TMP_InputField signUpPassword;
    private string getSignUpPassword = "SignUpPassword";
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
    [Header("LoadScene")]
    public int levelIndex = 1;
    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadTMPInputField();
        this.LoadGameObject();
    }
    protected virtual void LoadTMPInputField()
    {
        if (signUpUserName == null) signUpUserName = LoadTMPInputField(signUpUserName, getSignUpUserName);
        if (signUpEmail == null) signUpEmail = LoadTMPInputField(signUpEmail, getSignUpEmail);
        if (signUpPassword == null) signUpPassword = LoadTMPInputField(signUpPassword, getSignUpPassword); if (signUpUserName == null) signUpUserName = LoadTMPInputField(signUpUserName, getSignUpUserName);
        if (signInEmail == null) signInEmail = LoadTMPInputField(signInEmail, getSignInEmail);
        if (signInPassword == null) signInPassword = LoadTMPInputField(signInPassword, getSignInPassword);
    }
    protected virtual void LoadGameObject()
    {
        if (signUpPanel == null) signUpPanel = LoadGameObject(signUpPanel, getSignUpPanel);
        if (signInPanel == null) signInPanel = LoadGameObject(signInPanel, getSignInPanel);
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
}
