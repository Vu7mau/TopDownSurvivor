using PlayFab.ClientModels;
using PlayFab;
using UnityEngine;
using System.Text.RegularExpressions;
using System;
using System.Collections;
public class AuthEmailSignUp : AuthManager
{
    public void SignUpWithEmail()
    {
        var request = new RegisterPlayFabUserRequest
        {
            Username = signUpUserName.text,
            Email = signUpEmail.text,
            Password = signUpPassword.text,
            RequireBothUsernameAndEmail = true
        };
        PlayerPrefs.SetString("Email", signUpEmail.text);
        PlayerPrefs.SetInt("IsVerified", 0);
        PlayerPrefs.SetString("OtpTime", DateTime.Now.ToString("o"));
        PlayerPrefs.Save();
        if (string.IsNullOrEmpty(signUpUserName.text))
        {
            NotificationUI.Instance.Show("Vui lòng nhập tên người dùng");
            return;
        }
        if (signUpUserName.text.Length < 5)
        {
            NotificationUI.Instance.Show("Tên người dùng phải có ít nhất 5 ký tự");
            return;
        }
        if(signUpUserName.text.Contains(" "))
        {
            NotificationUI.Instance.Show("Tên người dùng không được chứa khoảng trắng");
            return;
        }
        if (!Regex.IsMatch(signUpUserName.text, @"^[a-zA-Z0-9_]+$"))
        {
            NotificationUI.Instance.Show("Tên người dùng chỉ được gồm chữ cái, số và không dấu.\r\n.");
            return;
        }
        if (string.IsNullOrEmpty(signUpEmail.text))
        {
            NotificationUI.Instance.Show("Vui lòng nhập địa chỉ email");
            return;
        }
        if (!Regex.IsMatch(signUpEmail.text, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
        {
            NotificationUI.Instance.Show("Email không hợp lệ");
            return;
        }
        if (string.IsNullOrEmpty(signUpPassword.text))
        {
            NotificationUI.Instance.Show("Vui lòng nhập mật khẩu");
            return;
        }
        if (signUpPassword.text.Length < 6)
        {
            NotificationUI.Instance.Show("Mật khẩu phải có ít nhất 6 ký tự");
            return;
        }
        PlayFabClientAPI.RegisterPlayFabUser(request, OnSignUpSucces, OnErrorSignUp);
    }
    private void OnSignUpSucces(RegisterPlayFabUserResult result)
    {
        PlayerPrefs.SetInt("HasLoggedIn", 1);
        NotificationUI.Instance.Show("Đăng ký người dùng mới thành công", 2f, () => {
            MainMenuTwo.Instance.ExitPanelRegister();
            StartCoroutine(ClearInput(0.5f));
        });
        CharacterInformation.Instance.ShowCharacters();
        PlayFabClientAPI.UpdateUserTitleDisplayName(
            new UpdateUserTitleDisplayNameRequest
            {
                DisplayName = signUpUserName.text
            },
        result =>
        {
            Debug.Log("Cập nhật tên người dùng thành công");
        },
        error =>
        {
            Debug.LogError("Lỗi cập nhật tên người dùng: " + error.GenerateErrorReport());
        }
        );
    }
    private IEnumerator ClearInput(float delay)
    {
        yield return new WaitForSeconds(delay);
        signUpUserName.text = "";
        signUpEmail.text = "";
        signUpPassword.text = "";
    }
    private void OnErrorSignUp(PlayFabError error)
    {
        string errorMessage = "";
        switch (error.Error)
        {
            case PlayFabErrorCode.UsernameNotAvailable:
                errorMessage = "Tên người dùng đã được sử dụng.";
                break;
            case PlayFabErrorCode.InvalidEmailAddress:
                errorMessage = "Địa chỉ email không hợp lệ.";
                break;
            case PlayFabErrorCode.EmailAddressNotAvailable:
                errorMessage = "Địa chỉ email đã được sử dụng.";
                break;
            case PlayFabErrorCode.InvalidPassword:
                errorMessage = "Mật khẩu không hợp lệ.";
                break;
            default:
                errorMessage += error.ErrorMessage;
                break;
        }
        NotificationUI.Instance.Show(errorMessage);
    }
}
