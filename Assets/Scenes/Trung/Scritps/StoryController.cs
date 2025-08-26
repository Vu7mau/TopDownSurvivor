using PlayFab.ClientModels;
using PlayFab;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryController : VuMonoBehaviour
{
    [SerializeField] protected EnemyAIController enemyAI;

    protected override void OnEnable()
    {
        base.OnEnable();
        this.CheckLastEnemiesDeath();
    }

    protected virtual void CheckLastEnemiesDeath()
    {
        StartCoroutine(this.CheckLastEnemiesDeathRoutine());
    }

    IEnumerator CheckLastEnemiesDeathRoutine()
    {
        if (enemyAI != null)
        {
            EnemyHealth enemyHealth = enemyAI.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                yield return new WaitUntil(() => enemyHealth.Health <= 0);

                //do something
                this.UnlockSurvivalMode();
                this.UnloadToPlayfab();
            }
        }
    }

    protected virtual void UnlockSurvivalMode()
    {
        Debug.Log("Đã mở chế độ Sinh tồn!");
        ModeUnlockManager.UnlockSurviveMode();
        ModePanel.Instance?.RefreshUIInstant();
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
