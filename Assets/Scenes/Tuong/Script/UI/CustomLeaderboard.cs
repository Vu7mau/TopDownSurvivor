using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CustomLeaderboard : MonoBehaviour
{
    public static CustomLeaderboard Instance;
    private const string leaderBoardKey = "LocalLeaderboard";
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public List<PlayerScore> GetLeaderboard()
    {
        string json = PlayerPrefs.GetString(leaderBoardKey, "");
        if(string.IsNullOrEmpty(json))
        {
            return new List<PlayerScore>();
        }
        return JsonUtility.FromJson<ScoreListWrapper>(json).list;
    }
    public void AddScore(string playerName, int score, float time)
    {
        var list = GetLeaderboard();
        list.Add(new PlayerScore
        {
            name = playerName,
            score = score,
            time = Mathf.RoundToInt(time)
        });
        var sorterd = list.OrderByDescending(x => x.score)
                          .ThenBy(x => x.time)
                          .Take(10)
                          .ToList();
        SaveLeaderBoard(sorterd);
    }
    private void SaveLeaderBoard(List<PlayerScore> list)
    {
        var wrapper = new ScoreListWrapper { list = list };
        string json = JsonUtility.ToJson(wrapper);
        PlayerPrefs.SetString(leaderBoardKey, json);
        PlayerPrefs.Save();
    }
    [System.Serializable]
    private class ScoreListWrapper
    {
        public List<PlayerScore> list;
    }
}
