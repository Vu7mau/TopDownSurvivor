using PlayFab;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScoreManager : MonoBehaviour
{
    public static PlayerScoreManager Instance;
    public int totalScore = 0;
    private HashSet<int> scoredEnemies = new HashSet<int>();
    private bool hasSentScore = false; 

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
            return;

        scoredEnemies.Add(enemy.GetInstanceID());
        totalScore += value;
        Debug.Log("Điểm hiện tại: " + totalScore);
    }

    public void ResetScore()
    {
        totalScore = 0;
        scoredEnemies.Clear();
        PlayerPrefs.SetInt("currentScores", 0);
        PlayerPrefs.Save();
        hasSentScore = false; 
        Debug.Log("Điểm đã được đặt lại.");
    }

    public void SendFinalScore(int score)
    {
        if (hasSentScore) 
        {
            Debug.Log("Điểm đã được gửi, bỏ qua.");
            return;
        }

        hasSentScore = true;
        SendScoreToLeaderboard(score);
        Debug.Log("Gửi điểm cuối cùng: " + score);
    }

    private void OnApplicationQuit() 
    {
        if (totalScore > 0)
        {
            Debug.Log("Đã thoát game, gửi điểm lên hệ thống");
            int finalScore = PlayerPrefs.GetInt("currentScores", 0);
            int coins = PlayerPrefs.GetInt("currentCoins", 0);
            PlayerScoreManager.Instance?.SendFinalScore(finalScore + coins);
            PlayerScoreManager.Instance?.ResetScore();
        }
    }

    public void SendScoreToLeaderboard(int score)
    {
        string mode = PlayerPrefs.GetString("LastMode", "Campaign");
        if (mode == "Campaign")
        {
            if (PlayFabClientAPI.IsClientLoggedIn())
            {
                LeaderBoardCampaign.Instance?.SendScoreCampign(score);
                Debug.Log("Gửi điểm lên BXH Campaign: " + score);
            }
        }
        else if (mode == "Survive")
        {
            if (PlayFabClientAPI.IsClientLoggedIn())
            {
                LeaderBoardSurvive.Instance?.SendScoreSurvive(score);
                Debug.Log("Gửi điểm lên BXH Survive: " + score);
            }
        }
    }
}
