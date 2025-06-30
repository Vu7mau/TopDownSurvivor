using PlayFab.ClientModels;
using PlayFab;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LeaderBoardSurvive : MonoBehaviour
{
    private string leaderboardSurvive = "Survive";
    [Header("UI Leaderboard")]
    [SerializeField] private TextMeshProUGUI surviveNameText;
    [SerializeField] private TextMeshProUGUI surviveScoreText;
    public void GetLeaderBoardSurvive()
    {
        var request = new GetLeaderboardRequest
        {
            StatisticName = leaderboardSurvive,
            StartPosition = 0,
            MaxResultsCount = 10
        };
        PlayFabClientAPI.GetLeaderboard(request, OnLeaderboardSuccess, OnLeaderboardError);
    }
    void OnLeaderboardSuccess(GetLeaderboardResult result)
    {
        surviveNameText.text = "";
        surviveScoreText.text = "";
        for (int i = 0; i < result.Leaderboard.Count && i < 10; i++)
        {
            var entry = result.Leaderboard[i];
            string displayName = string.IsNullOrEmpty(entry.DisplayName) ? "No name" : entry.DisplayName;
            surviveNameText.text += displayName + "\n";
            surviveScoreText.text += entry.StatValue.ToString() + "\n";
        }
    }
    void OnLeaderboardError(PlayFabError error)
    {
        Debug.LogError("Lỗi khi lấy bảng xếp hạng: " + error.GenerateErrorReport());
    }
    public void SendScoreSurvive(int value)
    {
        var request = new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate>
            {
                new StatisticUpdate
                {
                    StatisticName = leaderboardSurvive,
                    Value = value
                }
            }
        };
        PlayFabClientAPI.UpdatePlayerStatistics(request, OnUpdateStatisticsSuccess, OnUpdateStatisticsError);
    }
    void OnUpdateStatisticsSuccess(UpdatePlayerStatisticsResult result)
    {
        Debug.Log("Cập nhật điểm số thành công");
        GetLeaderBoardSurvive();
    }
    void OnUpdateStatisticsError(PlayFabError error)
    {
        Debug.Log("Lỗi" + error.GenerateErrorReport());
    }
}
