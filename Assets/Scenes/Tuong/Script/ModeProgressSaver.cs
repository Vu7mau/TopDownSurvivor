using PlayFab.ClientModels;
using PlayFab;
using System.Collections.Generic;
using UnityEngine;

public class ModeProgressSaver : MonoBehaviour
{
    private void OnEnable()
    {
        UnloadToPlayfab();
    }
    public void UnloadToPlayfab()
    {

        string playFabId = PlayFab.PlayFabSettings.staticPlayer.PlayFabId;

        if (string.IsNullOrEmpty(playFabId))
        {
            Debug.LogWarning("Không có PlayFabId, chưa login.");
            return;
        }

        var updateRequest = new UpdateUserDataRequest
        {
            Data = new Dictionary<string, string>
            {
                { "SurviveModeUnlocked", "1" },  
                { "LastUnlockTime", System.DateTime.UtcNow.ToString("o") }
            }
        };

        PlayFabClientAPI.UpdateUserData(updateRequest,
            result =>
            {
                Debug.Log("[PlayFab] Tiến trình Sinh tồn đã được lưu cho user: " + playFabId);
            },
            error =>
            {
                Debug.LogError("[PlayFab] Lưu tiến trình Sinh tồn thất bại: " + error.GenerateErrorReport());
            });
    }
}

