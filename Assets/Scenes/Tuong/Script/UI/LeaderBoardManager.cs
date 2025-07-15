using System.Linq;
using TMPro;
using UnityEngine;
public class LeaderBoardManager : MonoBehaviour
{
    [SerializeField] protected TextMeshProUGUI campaignNameText;
    [SerializeField] protected TextMeshProUGUI campaignScoreText;
    [SerializeField] protected TextMeshProUGUI campaignTimeText;
    [SerializeField] protected TextMeshProUGUI surviveNameText;
    [SerializeField] protected TextMeshProUGUI surviveScoreText;
    [SerializeField] protected TextMeshProUGUI surviveTimeText;
    [SerializeField] protected TextMeshProUGUI playerRankCampignText;
    [SerializeField] protected TextMeshProUGUI playerRankSurviveText;
    [SerializeField] protected RectTransform hightLightCampign;
    [SerializeField] protected RectTransform hightLightSurvive;
    [SerializeField] protected Canvas canvas;

    protected const string leaderboardStat = "CampaignManual";
    protected const string timeStat = "CampaignTime";
    protected const string leaderboardSurvive = "SurviveManual";
    protected const string timeSurvive = "SurviveTime";
    protected void RebindText()
    {
        var allTexts = Resources.FindObjectsOfTypeAll<TextMeshProUGUI>();

        campaignNameText = allTexts.FirstOrDefault(t => t.name == "CampaignNameText");
        campaignScoreText = allTexts.FirstOrDefault(t => t.name == "CampaignScoreText");
        campaignTimeText = allTexts.FirstOrDefault(t => t.name == "CampaignTimeText");
        surviveNameText = allTexts.FirstOrDefault(t => t.name == "SurviveNameText");
        surviveScoreText = allTexts.FirstOrDefault(t => t.name == "SurviveScoreText");
        surviveTimeText = allTexts.FirstOrDefault(t => t.name == "SurviveTimeText");
        playerRankCampignText = allTexts.FirstOrDefault(t => t.name == "PlayerRankCampaignText");
        playerRankSurviveText = allTexts.FirstOrDefault(t => t.name == "PlayerRankSurviveText");
        var allRects = Resources.FindObjectsOfTypeAll<RectTransform>();
        hightLightCampign = allRects.FirstOrDefault(t => t.name == "HightLightCampign");
        hightLightSurvive = allRects.FirstOrDefault(t => t.name == "HightLightSurvive");
        var allCanvases = Resources.FindObjectsOfTypeAll<Canvas>();
        canvas = allCanvases.FirstOrDefault(t => t.name == "Canvas");
    }
}
