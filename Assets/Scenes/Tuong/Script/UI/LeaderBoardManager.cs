using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class LeaderBoardManager : MonoBehaviour
{
    public static LeaderBoardManager Instance;
    [SerializeField] protected TextMeshProUGUI playerRankCampignText;
    [SerializeField] protected TextMeshProUGUI playerRankSurviveText;
    [SerializeField] protected RectTransform hightLightCampign;
    [SerializeField] protected RectTransform hightLightSurvive;
    [SerializeField] protected Canvas canvas;
    [SerializeField] protected Transform contentCampign;
    [SerializeField] protected Transform contentSurvive;
    public ScrollRect scrollRectCampign; 
    public ScrollRect scrollRectSurvive; 
    protected const string leaderboardStat = "CampaignManual";
    protected const string timeStat = "CampaignTime";
    protected const string leaderboardSurvive = "SurviveManual";
    protected const string timeSurvive = "SurviveTime";
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            RebindText();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    protected void RebindText()
    {
        var allTexts = Resources.FindObjectsOfTypeAll<TextMeshProUGUI>();
        playerRankCampignText = allTexts.FirstOrDefault(t => t.name == "PlayerRankCampaignText");
        playerRankSurviveText = allTexts.FirstOrDefault(t => t.name == "PlayerRankSurviveText");
        var allRects = Resources.FindObjectsOfTypeAll<RectTransform>();
        hightLightCampign = allRects.FirstOrDefault(t => t.name == "HightLightCampign");
        hightLightSurvive = allRects.FirstOrDefault(t => t.name == "HightLightSurvive");
        var allCanvases = Resources.FindObjectsOfTypeAll<Canvas>();
        canvas = allCanvases.FirstOrDefault(t => t.name == "Canvas");
        var allTransforms = Resources.FindObjectsOfTypeAll<Transform>();
        contentCampign = allTransforms.FirstOrDefault(t => t.name == "ContentCampign");
        contentSurvive = allTransforms.FirstOrDefault(t => t.name == "ContentSurvive");
        var allScrollRects = Resources.FindObjectsOfTypeAll<ScrollRect>();
        scrollRectCampign = allScrollRects.FirstOrDefault(t => t.name == "ScrollRectCampign");
        scrollRectSurvive = allScrollRects.FirstOrDefault(t => t.name == "ScrollRectSurvive");
    }
}
