using PlayFab;
using PlayFab.ClientModels;
using System.Text.RegularExpressions;
using UnityEngine;
public class ResetPassword : AuthManager
{
    public void SendRecoveryEmail()
    {
        if (string.IsNullOrEmpty(emailInputField.text))
        {
            NotificationUI.Instance.Show("Vui lòng nhập địa chỉ email");
            return;
        }
        if (!Regex.IsMatch(emailInputField.text, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
        {
            NotificationUI.Instance.Show("Email không hợp lệ.");
            return;
        }
        var request = new SendAccountRecoveryEmailRequest
        {
            Email = emailInputField.text,
            TitleId = PlayFabSettings.staticSettings.TitleId
        };
        PlayFabClientAPI.SendAccountRecoveryEmail(request, OnSendRecoveryEmailSuccess, OnSendRecoveryEmailError);
    }
    private void OnSendRecoveryEmailSuccess(SendAccountRecoveryEmailResult result)
    {
        NotificationUI.Instance.Show("Email khôi phục đã được gửi đến " + 
            "Vui lòng kiểm tra hộp thư và đổi mật khẩu, sau đó quay lại đăng nhập.", 7f, () =>
            {
                NotificationUI.Instance.HideImmediately();
                MainMenuTwo.Instance.EffectResetPassword.HidePanel(() =>
                {
                    MainMenuTwo.Instance.LogoutButton.SetActive(PlayerPrefs.GetInt("HasLoggedIn", 0) == 1);
                    MainMenuTwo.Instance.ResetPanel.SetActive(false);
                    MainMenuTwo.Instance.LoginPanel.SetActive(true);
                    MainMenuTwo.Instance.EffectLogin.ShowPanel();
                    
                    MainMenuTwo.Instance.authManager.signInEmail.text = emailInputField.text;
                    MainMenuTwo.Instance.StartCoroutine(MainMenuTwo.Instance.ClearInputResetPassword(0.5f));    

                });
                MainMenuTwo.Instance.OpenPanelLogin();
                signInEmail.text = emailInputField.text;
            });
    }
    private void OnSendRecoveryEmailError(PlayFabError error)
    {
        NotificationUI.Instance.Show("Lỗi khi gửi email khôi phục", 3f);
    }
}
