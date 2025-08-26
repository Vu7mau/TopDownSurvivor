using PlayFab.ClientModels;
using PlayFab;
using UnityEngine;

public class SurviveModeTracker : MonoBehaviour
{
    public static SurviveModeTracker Instance;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void LoadModeProgress()
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest(),
            result =>
            {
                bool surviveUnlocked = false;

                if (result.Data == null || !result.Data.ContainsKey("SurviveModeUnlocked"))
                {
                    Debug.Log("[SurviveModeTracker] Tài khoản mới, chưa có dữ liệu chế độ Sinh tồn.");
                    surviveUnlocked = false;

                    SaveSurviveModeStatus(false);
                }
                else if (result.Data.ContainsKey("SurviveModeUnlocked") && result.Data["SurviveModeUnlocked"].Value == "1")
                {
                    surviveUnlocked = true;
                }

                UpdateGameModeStatus(surviveUnlocked);
            },
            error =>
            {
                Debug.LogError("[SurviveModeTracker] Load dữ liệu chế độ thất bại: " + error.GenerateErrorReport());
            });
    }

    private void UpdateGameModeStatus(bool surviveUnlocked)
    {
        if (surviveUnlocked)
        {
            ModeUnlockManager.UnlockSurviveMode();
            Debug.Log("[SurviveModeTracker] Chế độ Sinh tồn đã mở cho người chơi.");
        }
        else
        {
            ModeUnlockManager.ResetUnlocks();
            Debug.Log("[SurviveModeTracker] Chế độ Sinh tồn chưa mở.");
        }

        ModePanel.Instance?.RefreshUIInstant();
    }
    public void SaveSurviveModeStatus(bool unlocked)
    {
        var request = new UpdateUserDataRequest
        {
            Data = new System.Collections.Generic.Dictionary<string, string>
            {
                { "SurviveModeUnlocked", unlocked ? "1" : "0" }
            }
        };
        PlayFabClientAPI.UpdateUserData(request,
            result => Debug.Log("[SurviveModeTracker] Đã lưu trạng thái Survival lên PlayFab."),
            error => Debug.LogError("[SurviveModeTracker] Lưu trạng thái Survival thất bại: " + error.GenerateErrorReport())
        );
    }
}
