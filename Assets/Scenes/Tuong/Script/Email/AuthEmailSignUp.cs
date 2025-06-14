﻿using PlayFab.ClientModels;
using PlayFab;
using UnityEngine.SceneManagement;
using UnityEngine;
using System.Text.RegularExpressions;
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
        PlayerPrefs.Save();
        if (string.IsNullOrEmpty(signUpUserName.text))
        {
            message.text = "Vui lòng nhập tên người dùng";
            return;
        }
        if (signUpUserName.text.Length < 5)
        {
            message.text = "Tên người dùng phải có ít nhất 5 ký tự";
            return;
        }
        if (string.IsNullOrEmpty(signUpEmail.text))
        {
            message.text = "Vui lòng nhập địa chỉ email";
            return;
        }
        if (!Regex.IsMatch(signUpEmail.text, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
        {
            message.text = "Email phải hợp lệ";
            return;
        }
        if (string.IsNullOrEmpty(signUpPassword.text))
        {
            message.text = "Vui lòng nhập mật khẩu";
            return;
        }
        if (signUpPassword.text.Length < 6)
        {
            message.text = "Mật khẩu phải có ít nhất 6 ký tự";
            return;
        }
        PlayFabClientAPI.RegisterPlayFabUser(request, OnSignUpSucces, OnErrorSignUp);
    }
    private void OnSignUpSucces(RegisterPlayFabUserResult result)
    {
        EmailVerificationSender.Instance.SendOTPEmal(signUpEmail.text,
            onSuccess: () =>
            {
                message.text = "Đăng ký người dùng mới thành công. Vui lòng kiểm tra email của bạn để xác minh tài khoản.";
                otpPanel.SetActive(true);
                signUpPanel.SetActive(false);
            },
            onFailure: (error) =>
            {
                message.text = "Lỗi gửi email xác minh: " + error;
            }
            );
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
    private void OnErrorSignUp(PlayFabError error)
    {
        string errorMessage = "Đăng ký thất bại: ";
        switch (error.Error)
        {
            case PlayFabErrorCode.InvalidEmailAddress:
                errorMessage += "Địa chỉ email không hợp lệ.";
                break;
            case PlayFabErrorCode.EmailAddressNotAvailable:
                errorMessage += "Địa chỉ email đã được sử dụng.";
                break;
            case PlayFabErrorCode.UsernameNotAvailable:
                errorMessage += "Tên người dùng đã được sử dụng.";
                break;
            case PlayFabErrorCode.InvalidPassword:
                errorMessage += "Mật khẩu không hợp lệ.";
                break;
            default:
                errorMessage += error.ErrorMessage;
                break;
        }
        message.text = errorMessage;
    }
    public void CheckOTP()
    {
        if (string.IsNullOrEmpty(otp.text))
        {
            message.text = "Vui lòng nhập mã OTP";
            return;
        }
        if (otp.text.Length < 6)
        {
            message.text = "Chưa đủ 6 ký tự";
            return;
        }
        bool isVerified = EmailVerificationSender.Instance.VerifyOTP(otp.text);
        if (isVerified)
        {
            AuthEmailSignIn authEmailSignIn = FindObjectOfType<AuthEmailSignIn>();
            authEmailSignIn.LinkDeviceAndProceed();
        }

        else message.text = "Mã OTP không chính xác";
    }
}
