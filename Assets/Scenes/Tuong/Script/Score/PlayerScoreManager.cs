using PlayFab;
using System.Collections.Generic;
using UnityEngine;
public class PlayerScoreManager : MonoBehaviour
{
    public static PlayerScoreManager Instance;
    public int totalScore = 0;
    private HashSet<int> scoredEnemies = new HashSet<int>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }
    public void AddScore(GameObject enemy, int value)
    {
        if (scoredEnemies.Contains(enemy.GetInstanceID()))
            return; Debug.Log("Điểm hiện tại: " + totalScore);
        scoredEnemies.Add(enemy.GetInstanceID());
        totalScore += value;
    }
    public void ResetScore()
    {
        PlayerPrefs.SetInt("currentScores", 0);
        PlayerPrefs.Save();
        Debug.Log("Điểm đã được đặt lại.");
    }
    public void SendFinalScore(int score)
    {
        SendScoreToLeaderboard(score);
        Debug.Log("Gửi điểm cuối cùng: " + totalScore);
    }
    private void ApplicationQuit()
    {
        if(totalScore > 0)
        {
            Debug.Log("Đã thoát game, gửi điểm lên hệ thống");
            int finalScore = PlayerPrefs.GetInt("currentScores", 0);
            SendFinalScore(finalScore);
        }
    }
    public void SendScoreToLeaderboard(int score)
    {
        string mode = PlayerPrefs.GetString("LastMode", "Campaign"); 
        if(mode == "Campaign")
        {
            if (PlayFabClientAPI.IsClientLoggedIn())
            {
                LeaderBoardCampaign.Instance?.SendScoreCampign(score);
                Debug.Log("Gửi điểm lên bxh Campign: " + score);
            }
        }
        else if(mode == "Survive")
        {
            if (PlayFabClientAPI.IsClientLoggedIn())
            {
                LeaderBoardSurvive.Instance?.SendScoreSurvive(score);
                Debug.Log("Gửi điểm lên bxh Survive: " + score);
            }
        }
    }
}
