using System.Linq;
using TMPro;
using UnityEngine;
public class LeaderBoardManager : MonoBehaviour
{
    [SerializeField] protected TextMeshProUGUI campaignNameText;
    [SerializeField] protected TextMeshProUGUI campaignScoreText;
    [SerializeField] protected TextMeshProUGUI surviveNameText;
    [SerializeField] protected TextMeshProUGUI surviveScoreText;
    [SerializeField] protected TextMeshProUGUI playerRankCampignText;
    [SerializeField] protected TextMeshProUGUI playerRankSurviveText;
    protected const string leaderboardStat = "CampaignManual";
    protected const string timeStat = "CampaignTime";
    protected const string leaderboardSurvive = "SurviveManual";
    protected const string timeSurvive = "SurviveTime";
    protected void RebindText()
    {
        var allTexts = Resources.FindObjectsOfTypeAll<TextMeshProUGUI>();

        campaignNameText = allTexts.FirstOrDefault(t => t.name == "CampaignNameText");
        campaignScoreText = allTexts.FirstOrDefault(t => t.name == "CampaignScoreText");
        surviveNameText = allTexts.FirstOrDefault(t => t.name == "SurviveNameText");
        surviveScoreText = allTexts.FirstOrDefault(t => t.name == "SurviveScoreText");
        playerRankCampignText = allTexts.FirstOrDefault(t => t.name == "PlayerRankCampaignText");
        playerRankSurviveText = allTexts.FirstOrDefault(t => t.name == "PlayerRankSurviveText");
    }
}
