using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;
using DG.Tweening;
public class LeaderBoardCampaign : LeaderBoardManager
{
    [SerializeField] private GameObject entryPrefabCampign;
    public new static LeaderBoardCampaign Instance;
    private Dictionary<string, DateTime> createdMapCache = new(); // Dictionary là bảng ánh xạ, id - ngày tạo tài khoản
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }
    public void GetMyRank()
    {
        if (!PlayFabClientAPI.IsClientLoggedIn())
        {
            Debug.LogWarning("Chưa đăng nhập vào PlayFab, không thể lấy bảng xếp hạng.");
            return;
        }
        PlayFabClientAPI.GetLeaderboardAroundPlayer(new GetLeaderboardAroundPlayerRequest
        {
            StatisticName = leaderboardCampign,
            MaxResultsCount = 1
        }, result =>
        {
            if (result.Leaderboard?.Count > 0) StartCoroutine(LoadRank(result.Leaderboard[0]));
            else rankCampaign.text = "Không tìm thấy thứ hạng.";
        }, error =>
        {
            rankCampaign.text = "Lỗi khi lấy hạng.";
            Debug.LogError("Lỗi lấy bảng xếp hạng xung quanh người chơi: " + error.GenerateErrorReport());
        });
    }
    private IEnumerator LoadRank(PlayerLeaderboardEntry myEntry)
    {
        var topEntries = new List<PlayerLeaderboardEntry>();
        var timeMap = new Dictionary<string, int>();
        var createdMap = new Dictionary<string, DateTime>();
        bool doneScores = false;
        PlayFabClientAPI.GetLeaderboard(new GetLeaderboardRequest
        {
            StatisticName = leaderboardCampign,
            StartPosition = 0,
            MaxResultsCount = 100
        },
        res => { topEntries = res.Leaderboard.ToList(); doneScores = true; },
        err => { Debug.LogError(err.GenerateErrorReport()); doneScores = true; });
        yield return new WaitUntil(() => doneScores);
        if (!topEntries.Any(e => e.PlayFabId == myEntry.PlayFabId))
            topEntries.Add(myEntry);
        bool doneTimes = false;
        PlayFabClientAPI.GetLeaderboard(new GetLeaderboardRequest
        {
            StatisticName = timeCampign,
            StartPosition = 0,
            MaxResultsCount = 100
        }, res => { foreach (var e in res.Leaderboard) timeMap[e.PlayFabId] = e.StatValue; doneTimes = true; },
        err => { Debug.LogWarning(err.GenerateErrorReport()); doneTimes = true; });
        yield return new WaitUntil(() => doneTimes);
        if (!timeMap.ContainsKey(myEntry.PlayFabId))
        {
            bool doneSelfTime = false;
            PlayFabClientAPI.GetPlayerStatistics(new GetPlayerStatisticsRequest
            {
                StatisticNames = new List<string> { timeCampign }
            }, res =>
            {
                var stat = res.Statistics.FirstOrDefault(s => s.StatisticName == timeCampign);
                timeMap[myEntry.PlayFabId] = stat?.Value ?? int.MaxValue; doneSelfTime = true;
            }, err => { timeMap[myEntry.PlayFabId] = int.MaxValue; doneSelfTime = true; });
            yield return new WaitUntil(() => doneSelfTime);
        }
        int pending = topEntries.Count;
        bool doneAccounts = false;
        foreach (var entry in topEntries)
        {
            string id = entry.PlayFabId;
            PlayFabClientAPI.GetAccountInfo(new GetAccountInfoRequest { PlayFabId = id },
            res =>
            {
                createdMap[id] = res.AccountInfo.Created; createdMapCache[id] = res.AccountInfo.Created;
                if (--pending == 0) doneAccounts = true;
            }, err =>
            {
                createdMap[id] = DateTime.MaxValue;
                if (--pending == 0) doneAccounts = true;
            });
        }
        yield return new WaitUntil(() => doneAccounts);
        topEntries.Sort((a, b) => CompareRank(a, b, timeMap, createdMap));
        int myIndex = topEntries.FindIndex(e => e.PlayFabId == myEntry.PlayFabId);
        string name = string.IsNullOrEmpty(myEntry.DisplayName) ? "Bạn" : myEntry.DisplayName;
        nameCampaign.text = $"{name}";
        rankCampaign.text = $"{myIndex + 1}";
        var foundEntry = topEntries.Find(e => e.PlayFabId == myEntry.PlayFabId);
        int score = foundEntry != null ? foundEntry.StatValue : 0;
        scoreCampaign.text = $"{myEntry.StatValue}";

        if (timeMap.TryGetValue(myEntry.PlayFabId, out int playTime))
            timeCampignTwo.text = FormatTime(playTime);
        else
            timeCampignTwo.text = "??:??";
    }
    public void GetLeaderBoardCampaign()
    {
        if (!PlayFabClientAPI.IsClientLoggedIn())
        {
            Debug.LogWarning("Chưa đăng nhập vào PlayFab, không thể lấy bảng xếp hạng.");
            return;
        }
        RebindText();
        PlayFabClientAPI.GetLeaderboard(new GetLeaderboardRequest
        {
            StatisticName = leaderboardCampign,
            StartPosition = 0,
            MaxResultsCount = 20
        },
        result => StartCoroutine(GetLeaderboardTime(result.Leaderboard)),
        error => Debug.LogError("Lỗi khi lấy bảng xếp hạng: " + error.GenerateErrorReport()));
    }
    private IEnumerator GetLeaderboardTime(List<PlayerLeaderboardEntry> entries)
    {
        Dictionary<string, int> timeMap = new();
        Dictionary<string, DateTime> createdMap = new();
        bool doneTime = false;
        bool doneCreated = false;
        PlayFabClientAPI.GetLeaderboard(new GetLeaderboardRequest
        {
            StatisticName = timeCampign,
            StartPosition = 0,
            MaxResultsCount = 100
        },
        result =>
        {
            foreach (var e in result.Leaderboard)
                timeMap[e.PlayFabId] = e.StatValue;
            doneTime = true;
        },
        error => { doneTime = true; });
        yield return new WaitUntil(() => doneTime);
        int pending = entries.Count;
        foreach (var entry in entries)
        {
            string id = entry.PlayFabId;
            if (createdMapCache.TryGetValue(id, out DateTime cached))
            {
                createdMap[id] = cached;
                if (--pending == 0) doneCreated = true;
                continue;
            }
            PlayFabClientAPI.GetAccountInfo(new GetAccountInfoRequest { PlayFabId = id },
            res =>
            {
                createdMap[id] = res.AccountInfo.Created;
                createdMapCache[id] = res.AccountInfo.Created;
                if (--pending == 0) doneCreated = true;
            },
            err =>
            {
                createdMap[id] = DateTime.MaxValue;
                if (--pending == 0) doneCreated = true;
            });
        }
        yield return new WaitUntil(() => doneCreated);
        entries.Sort((a, b) => CompareRank(a, b, timeMap, createdMap));
        DisplayLeaderboard(entries, timeMap);
    }
    private static int CompareRank(PlayerLeaderboardEntry a, PlayerLeaderboardEntry b, Dictionary<string, int> timeMap, Dictionary<string, DateTime> createdMap)
    {
        int cmp = b.StatValue.CompareTo(a.StatValue);
        if (cmp != 0) return cmp;
        timeMap.TryGetValue(a.PlayFabId, out int ta);
        timeMap.TryGetValue(b.PlayFabId, out int tb);
        cmp = ta.CompareTo(tb);
        if (cmp != 0) return cmp;
        createdMap.TryGetValue(a.PlayFabId, out DateTime ca);
        createdMap.TryGetValue(b.PlayFabId, out DateTime cb);
        return ca.CompareTo(cb);
    }
    private void DisplayLeaderboard(List<PlayerLeaderboardEntry> entries, Dictionary<string, int> timeMap)
    {
        if (contentCampign != null)
        {
            foreach (Transform child in contentCampign)
            {
                if (child != null)
                {
                    child.DOKill();
                    Destroy(child.gameObject);
                }
            }
        }
        string currentId = PlayFabSettings.staticPlayer.PlayFabId;
        if (entries.Count > 0)
        {
            if(top1CampaignNameText != null)
            {
                top1CampaignNameText.text = entries[0].DisplayName ?? "No name";
            }
            if (top1CampaignScoreText != null)
            {
                top1CampaignScoreText.text = entries[0].StatValue.ToString();
            }
        }
        if (entries.Count > 1)
        {
            if (top2CampaignNameText != null)
            {
                top2CampaignNameText.text = entries[1].DisplayName ?? "No name";
            }
            if (top2CampaignScoreText != null)
            {
                top2CampaignScoreText.text = entries[1].StatValue.ToString();
            }
        }
        if (entries.Count > 2)
        {
            if (top3CampaignNameText != null)
            {
                top3CampaignNameText.text = entries[2].DisplayName ?? "No name";
            }
            if (top3CampaignScoreText != null)
            {
                top3CampaignScoreText.text = entries[2].StatValue.ToString();
            }
        }
        for (int i = 3; i < entries.Count; i++)
        {
            var e = entries[i];

            GameObject go = Instantiate(entryPrefabCampign, contentCampign);
            var texts = go.GetComponentsInChildren<TextMeshProUGUI>();
            texts[0].text = (i + 1).ToString();
            texts[1].text = e.DisplayName ?? "No name";
            string time = timeMap.TryGetValue(e.PlayFabId, out int t) ? FormatTime(t) : "??:??";
            texts[2].text = time;
            texts[3].text = e.StatValue.ToString();

        }
    }
    private string FormatTime(int totalSeconds)
    {
        if (totalSeconds <= 0)
            return "0s";

        int days = totalSeconds / 86400;
        int hours = (totalSeconds % 86400) / 3600;
        int minutes = (totalSeconds % 3600) / 60;
        int seconds = totalSeconds % 60;

        if (days > 0)
            return $"{days}d {hours}h {minutes}m {seconds}s";
        else if (hours > 0)
            return $"{hours}h {minutes}m {seconds}s";
        else if (minutes > 0)
            return $"{minutes}m {seconds}s";
        else
            return $"{seconds}s";
    }
    public void SendScoreCampign(int deltaScore)
    {
        if (!PlayFabClientAPI.IsClientLoggedIn())
        {
            Debug.LogWarning("Chưa đăng nhập PlayFab.");
            return;
        }

        int sessionTime = CountDownTimer.Instance?.GetSessionDurationInSeconds() ?? 0;

        var stats = new List<StatisticUpdate>
        {
            new StatisticUpdate { StatisticName = leaderboardCampign, Value = deltaScore },
            new StatisticUpdate { StatisticName = timeCampign, Value = sessionTime }
        };

        PlayFabClientAPI.UpdatePlayerStatistics(
            new UpdatePlayerStatisticsRequest { Statistics = stats },
            _ =>
            {
                Debug.Log($"[PlayFab] +{deltaScore} điểm, +{sessionTime}s (Aggregation=Sum)");
                GetLeaderBoardCampaign();
            },
            err => Debug.LogError("Lỗi gửi điểm: " + err.GenerateErrorReport())
        );
    }
    public void EnsureDefaultScore()
    {
        var stats = new List<StatisticUpdate>
        {
            new() { StatisticName = leaderboardCampign, Value = 0 },
            new() { StatisticName = timeCampign, Value = 0 }
        };
        PlayFabClientAPI.UpdatePlayerStatistics(new UpdatePlayerStatisticsRequest { Statistics = stats },
            result => Debug.Log("Đã đăng ký điểm 0 ban đầu"),
            error => Debug.LogError("Lỗi gửi điểm mặc định: " + error.GenerateErrorReport()));
    }
}