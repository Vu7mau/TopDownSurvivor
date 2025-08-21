using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;
using DG.Tweening;
using UnityEngine.SceneManagement;
public class LeaderBoardSurvive : LeaderBoardManager
{
    [SerializeField] private GameObject entryPrefabSurvive;
    public new static LeaderBoardSurvive Instance;
    private Dictionary<string, DateTime> createdMapCache = new();

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
            StatisticName = leaderboardSurvive,
            MaxResultsCount = 1
        }, result =>
        {
            if (result.Leaderboard?.Count > 0) StartCoroutine(LoadRank(result.Leaderboard[0]));
            else rankSurvive.text = "Không tìm thấy thứ hạng.";
        }, error =>
        {
            rankSurvive.text = "Lỗi khi lấy hạng.";
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
            StatisticName = leaderboardSurvive,
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
            StatisticName = timeSurvive,
            StartPosition = 0,
            MaxResultsCount = 100
        },
        res => { foreach (var e in res.Leaderboard) timeMap[e.PlayFabId] = e.StatValue; doneTimes = true; },
        err => { Debug.LogWarning(err.GenerateErrorReport()); doneTimes = true; });
        yield return new WaitUntil(() => doneTimes);

        if (!timeMap.ContainsKey(myEntry.PlayFabId))
        {
            bool doneSelfTime = false;
            PlayFabClientAPI.GetPlayerStatistics(new GetPlayerStatisticsRequest
            {
                StatisticNames = new List<string> { timeSurvive }
            }, res =>
            {
                var stat = res.Statistics.FirstOrDefault(s => s.StatisticName == timeSurvive);
                timeMap[myEntry.PlayFabId] = stat?.Value ?? int.MaxValue;
                doneSelfTime = true;
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
        nameSurvive.text = $"{name}";
        rankSurvive.text = $"{myIndex + 1}";

        var foundEntry = topEntries.Find(e => e.PlayFabId == myEntry.PlayFabId);
        int score = foundEntry != null ? foundEntry.StatValue : 0;
        scoreSurvive.text = $"{score}";

        if (timeMap.TryGetValue(myEntry.PlayFabId, out int playTime))
            timeSurviveTwo.text = FormatTime(playTime);
        else
            timeSurviveTwo.text = "??:??";
    }

    public void GetLeaderBoardSurvive()
    {
        if (!PlayFabClientAPI.IsClientLoggedIn())
        {
            Debug.LogWarning("Chưa đăng nhập vào PlayFab, không thể lấy bảng xếp hạng.");
            return;
        }
        RebindText();
        PlayFabClientAPI.GetLeaderboard(new GetLeaderboardRequest
        {
            StatisticName = leaderboardSurvive,
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
            StatisticName = timeSurvive,
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
        cmp = tb.CompareTo(ta);
        if (cmp != 0) return cmp;
        createdMap.TryGetValue(a.PlayFabId, out DateTime ca);
        createdMap.TryGetValue(b.PlayFabId, out DateTime cb);
        return ca.CompareTo(cb);
    }
    private void DisplayLeaderboard(List<PlayerLeaderboardEntry> entries, Dictionary<string, int> timeMap)
    {
        if (contentSurvive != null)
        {
            foreach (Transform child in contentSurvive)
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
            if (top1SurviveNameText != null)
            {
                top1SurviveNameText.text = entries[0].DisplayName ?? "No name";
            }
            if (top1SurviveScoreText != null)
            {
                top1SurviveScoreText.text = entries[0].StatValue.ToString();
            }
        }
        if (entries.Count > 1)
        {
            if (top2SurviveNameText != null)
            {
                top2SurviveNameText.text = entries[1].DisplayName ?? "No name";
            }
            if (top2SurviveScoreText != null)
            {
                top2SurviveScoreText.text = entries[1].StatValue.ToString();
            }
        }
        if (entries.Count > 2)
        {
            if (top3SurviveNameText != null)
            {
                top3SurviveNameText.text = entries[2].DisplayName ?? "No name";
            }
            if (top3SurviveScoreText != null)
            {
                top3SurviveScoreText.text = entries[2].StatValue.ToString();
            }
        }
        for (int i = 3; i < entries.Count; i++)
        {
            var e = entries[i];
            if (SceneManager.GetActiveScene().buildIndex == 1)
            {
                GameObject go = Instantiate(entryPrefabSurvive, contentSurvive);
                var texts = go.GetComponentsInChildren<TextMeshProUGUI>();
                texts[0].text = (i + 1).ToString();
                texts[1].text = e.DisplayName ?? "No name";
                string time = timeMap.TryGetValue(e.PlayFabId, out int t) ? FormatTime(t) : "??:??";
                texts[2].text = time;
                texts[3].text = e.StatValue.ToString();
            }

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
    public void SendScoreSurvive(int deltaScore)
    {
        if (!PlayFabClientAPI.IsClientLoggedIn())
        {
            Debug.LogWarning("Chưa đăng nhập PlayFab.");
            return;
        }

        int sessionTime = CountDownTimer.Instance?.GetSessionDurationInSeconds() ?? 0;

        var stats = new List<StatisticUpdate>
        {
            new StatisticUpdate { StatisticName = leaderboardSurvive, Value = deltaScore },
            new StatisticUpdate { StatisticName = timeSurvive, Value = sessionTime }
        };

        PlayFabClientAPI.UpdatePlayerStatistics(
            new UpdatePlayerStatisticsRequest { Statistics = stats },
            _ =>
            {
                Debug.Log($"[PlayFab] +{deltaScore} điểm, +{sessionTime}s (Aggregation=Sum)");
                GetLeaderBoardSurvive();
            },
            err => Debug.LogError("Lỗi gửi điểm: " + err.GenerateErrorReport())
        );
    }

    public void EnsureDefaultScore()
    {
        var stats = new List<StatisticUpdate>
            {
                new() { StatisticName = leaderboardSurvive, Value = 0 },
                new() { StatisticName = timeSurvive, Value = 0 }
            };
        PlayFabClientAPI.UpdatePlayerStatistics(new UpdatePlayerStatisticsRequest { Statistics = stats },
            result => Debug.Log("Đã đăng ký điểm 0 ban đầu"),
            error => Debug.LogError("Lỗi gửi điểm mặc định: " + error.GenerateErrorReport()));
    }
}