using PlayFab.ClientModels;
using PlayFab;
using UnityEngine.SceneManagement;
using UnityEngine;
using System.Text.RegularExpressions;
public class AuthEmailSignIn : AuthManager
{
    public MainMenuTwo mainMenuTwo;
    public void SignInGame()
    {
        if (string.IsNullOrEmpty(signInEmail.text))
        {
            message.text = "Vui lòng nhập địa chỉ email";
            return;
        }
        if (!Regex.IsMatch(signInEmail.text, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
        {
            message.text = "Email không hợp lệ";
            return;
        }
        if (string.IsNullOrEmpty(signInPassword.text))
        {
            message.text = "Vui lòng nhập mật khẩu";
            return;
        }
        var request = new LoginWithEmailAddressRequest
        {
            Email = signInEmail.text,
            Password = signInPassword.text
        };
        PlayFabClientAPI.LoginWithEmailAddress(request, OnSignInSuccess, OnErrorSignIn);
    }
    private void OnSignInSuccess(LoginResult result)
    {
        message.text = "Đăng nhập thành công";
        LinkDeviceAndProceed();
    }
    public void LinkDeviceAndProceed()
    {
        string deviceId = SystemInfo.deviceUniqueIdentifier;
        var linkRequest = new LinkCustomIDRequest
        {
            CustomId = deviceId,
            ForceLink = true
        };
        PlayFabClientAPI.LinkCustomID(linkRequest,
            success =>
            {
                Debug.Log("Đã liên kết tài khoản với thiết bị");
                OnLoginFinalized();
            },
            error =>
            {
                if (error.Error == PlayFabErrorCode.LinkedDeviceAlreadyClaimed)
                {
                    Debug.Log("Thiết bị đã liên kết, tiếp tục ");
                    OnLoginFinalized();
                }
                else
                {
                    Debug.Log("Liên kết thiết bị thất bại: " + error.ErrorMessage);
                }
            });
    }
    private void OnLoginFinalized()
    {
        PlayerPrefs.SetInt("HasLoggedIn", 1);
        PlayerPrefs.Save();
        LeaderBoardManager leaderBoardManager = FindObjectOfType<LeaderBoardManager>();
        if (leaderBoardManager != null)
        {
            leaderBoardManager.GetLeaderboard();
        }
        SceneManager.LoadScene(levelIndex);
    }
    private void OnErrorSignIn(PlayFabError error)
    {
        switch (error.Error)
        {
            case PlayFabErrorCode.AccountNotFound:
                message.text = "Không tìm thấy tài khoản";
                break;
            case PlayFabErrorCode.InvalidPassword:
                message.text = "Sai mật khẩu";
                break;
            case PlayFabErrorCode.InvalidEmailAddress:
                message.text = "Email không hợp lệ";
                break;
            case PlayFabErrorCode.AccountBanned:
                message.text = "Tài khoản bị khóa";
                break;
            default:
                message.text = "Đăng nhập thất bại: " + error.ErrorMessage;
                Debug.LogError($"Lỗi đăng nhập: {error.Error} - {error.ErrorMessage}");
                break;
        }
    }
}
