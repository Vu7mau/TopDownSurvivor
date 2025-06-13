using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
public class AutoLogin : MonoBehaviour
{
    public MainMenuTwo mainMenuTwo;
    private void Start()
    {
        bool autoLoginDisable = PlayerPrefs.GetInt("AutoLoginDisable", 0) == 1;
        if(!autoLoginDisable)
        {
            LoginWithCustomID();
        }
        else
        {
            mainMenuTwo.SetLoginState(false);
        }
    }
    private void LoginWithCustomID()
    {
        string deviceId = SystemInfo.deviceUniqueIdentifier;
        var request = new LoginWithCustomIDRequest
        {
            CustomId = deviceId,
            CreateAccount = false
        };
        PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnLoginFailured);
    }
    private void OnLoginSuccess(LoginResult result)
    {
        Debug.Log("Auto Login thành công");
        PlayerPrefs.SetInt("HasLoggedIn", 1);
        PlayerPrefs.Save();
        mainMenuTwo?.SetLoginState(true);
    }
    private void OnLoginFailured(PlayFabError error)
    {
        Debug.Log("Lỗi Auto đăng nhập");
        PlayerPrefs.SetInt("HasLoggedIn", 0);
        PlayerPrefs.Save();
        mainMenuTwo?.SetLoginState(false);
    }
}
