using TMPro;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.UI;
public class ResetPassword : AuthManager
{
    public void SendRecoveryEmail()
    {
        if(string.IsNullOrEmpty(emailInputField.text))
        {
            message.text = "Vui lòng nhập địa chỉ email";
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
        message.text = "Email khôi phục đã được gửi.";
    }
    private void OnSendRecoveryEmailError(PlayFabError error)
    {
        message.text = "Lỗi: " + error.ErrorMessage;
    }
}
