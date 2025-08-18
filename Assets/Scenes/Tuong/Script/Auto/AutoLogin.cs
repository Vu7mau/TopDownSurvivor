using System.Collections;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
public class AutoLogin : MonoBehaviour
{
    public static AutoLogin Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        Debug.Log("AutoLoginDisable = " + PlayerPrefs.GetInt("AutoLoginDisable"));        
        bool autoLoginDisable = PlayerPrefs.GetInt("AutoLoginDisable", 0) == 1;
        bool hasLoggedIn = PlayerPrefs.GetInt("HasLoggedIn", 0) == 1;
        if (!autoLoginDisable && !PlayFabClientAPI.IsClientLoggedIn())
        {
            LoginWithCustomID();
        }
    }

    public void LoginWithCustomID()
    {
        string deviceId = SystemInfo.deviceUniqueIdentifier;
        PlayerPrefs.SetString("CustomId", deviceId);
        PlayerPrefs.Save();
        var request = new LoginWithCustomIDRequest
        {
            CustomId = deviceId,
            CreateAccount = false
        };

        PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnLoginFailured);
    }

    private void OnLoginSuccess(LoginResult result)
    {
        Debug.Log("Auto Login PlayFab thành công. PlayFabId = " + result.PlayFabId);
        Debug.Log("IsClientLoggedIn SAU khi login = " + PlayFabClientAPI.IsClientLoggedIn());
        PlayerPrefs.SetInt("HasLoggedIn", 1);

        StartCoroutine(DeleyAndLoadLeaderBoard());
    }

    private IEnumerator DeleyAndLoadLeaderBoard()
    {
        yield return new WaitUntil(() =>
            PlayFabClientAPI.IsClientLoggedIn() &&
            LeaderBoardCampaign.Instance != null &&
            LeaderBoardSurvive.Instance != null);

        Debug.Log("Auto Login hoàn tất, đang tải LeaderBoard");
        LeaderBoardCampaign.Instance.GetLeaderBoardCampaign();
        LeaderBoardCampaign.Instance.GetMyRank();
        LeaderBoardSurvive.Instance.GetLeaderBoardSurvive();
        LeaderBoardSurvive.Instance.GetMyRank();
    }

    private void OnLoginFailured(PlayFabError error)
    {
        Debug.Log("Lỗi Auto đăng nhập: " + error.GenerateErrorReport());
        Debug.Log("Mã lỗi: " + error.Error.ToString());
        PlayerPrefs.SetInt("HasLoggedIn", 0);
        PlayerPrefs.Save();
    }
}