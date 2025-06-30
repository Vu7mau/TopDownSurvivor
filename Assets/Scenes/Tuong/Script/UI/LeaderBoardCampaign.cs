using PlayFab.ClientModels;
using PlayFab;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LeaderBoardCampaign : MonoBehaviour
{
    private string leaderboardCampaign = "Campaign";
    [Header("UI Leaderboard")]
    [SerializeField] private TextMeshProUGUI campaignNameText;
    [SerializeField] private TextMeshProUGUI campaignScoreText;
    public void GetLeaderBoardCampaign()
    {
        var request = new GetLeaderboardRequest
        {
            StatisticName = leaderboardCampaign,
            StartPosition = 0,
            MaxResultsCount = 10
        };
        PlayFabClientAPI.GetLeaderboard(request, OnLeaderboardSuccess, OnLeaderboardError);
    }
    void OnLeaderboardSuccess(GetLeaderboardResult result)
    {
        campaignNameText.text = "";
        campaignScoreText.text = "";
        for (int i = 0; i < result.Leaderboard.Count && i < 10; i++)
        {
            var entry = result.Leaderboard[i];
            string displayName = string.IsNullOrEmpty(entry.DisplayName) ? "No name" : entry.DisplayName;
            campaignNameText.text += displayName + "\n";
            campaignScoreText.text += entry.StatValue.ToString() + "\n";
        }
    }
    void OnLeaderboardError(PlayFabError error)
    {
        Debug.LogError("Lỗi khi lấy bảng xếp hạng: " + error.GenerateErrorReport());
    }
    public void SendScoreCampaign(int value)
    {
        var request = new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate>
            {
                new StatisticUpdate
                {
                    StatisticName = leaderboardCampaign,
                    Value = value
                }
            }
        };
        PlayFabClientAPI.UpdatePlayerStatistics(request, OnUpdateStatisticsSuccess, OnUpdateStatisticsError);
    }
    void OnUpdateStatisticsSuccess(UpdatePlayerStatisticsResult result)
    {
        Debug.Log("Cập nhật điểm số thành công");
        GetLeaderBoardCampaign();
    }
    void OnUpdateStatisticsError(PlayFabError error)
    {
        Debug.Log("Lỗi" + error.GenerateErrorReport());
    }
}
