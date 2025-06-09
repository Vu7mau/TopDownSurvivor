using PlayFab.ClientModels;
using PlayFab;
using UnityEngine.SceneManagement;
using UnityEngine;
public class AuthEmailSignIn : AuthManager
{
    public void SignInGame()
    {
        if (string.IsNullOrEmpty(signInEmail.text))
        {
            message.text = "Vui lòng nhập địa chỉ email";
            return;
        }
        if (!signInEmail.text.Contains("@gmail.com"))
        {
            message.text = "Email phải có đuôi @gmail.com";
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
