using PlayFab;
using PlayFab.ClientModels;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
public class LeaderBoardManager : MonoBehaviour
{
    private string leaderboardName = "MyLeaderboard";
    [Header("UI Leaderboard")]
    [SerializeField] private TextMeshProUGUI rankText;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI scoreText;
    public void GetLeaderboard()
    {
        var request = new GetLeaderboardRequest
        {
            StatisticName = leaderboardName,
            StartPosition = 0,
            MaxResultsCount = 10
        };
        PlayFabClientAPI.GetLeaderboard(request, OnLeaderboardSuccess, OnLeaderboardError);
    }

    void OnLeaderboardSuccess(GetLeaderboardResult result)
    {
        rankText.text = "";
        nameText.text = "";
        scoreText.text = "";
        for (int i = 0; i < result.Leaderboard.Count && i < 10; i++)
        {
            var entry = result.Leaderboard[i];
            string displayName = string.IsNullOrEmpty(entry.DisplayName) ? "No name" : entry.DisplayName;
            rankText.text += (entry.Position + 1).ToString() + "\n";
            nameText.text += displayName + "\n";
            scoreText.text += entry.StatValue.ToString() + "\n";
        }
    }
    void OnLeaderboardError(PlayFabError error)
    {
        Debug.LogError("Lỗi khi lấy bảng xếp hạng: " + error.GenerateErrorReport());
    }
    public void SendTestScore(int value)
    {
        var request = new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate>
            {
                new StatisticUpdate
                {
                    StatisticName = leaderboardName,
                    Value = value
                }
            }
        };
        PlayFabClientAPI.UpdatePlayerStatistics(request, OnUpdateStatisticsSuccess, OnUpdateStatisticsError);
    }
    void OnUpdateStatisticsSuccess(UpdatePlayerStatisticsResult result)
    {
        Debug.Log("Cập nhật điểm số thành công");
        GetLeaderboard();
    }
    void OnUpdateStatisticsError(PlayFabError error)
    {
        Debug.Log("Lỗi" + error.GenerateErrorReport());
    }
}
