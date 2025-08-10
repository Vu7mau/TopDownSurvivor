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
        totalScore = 0;
        Debug.Log("Điểm đã được đặt lại.");
    }
    public void SendFinalScore()
    {
        SendScoreToLeaderboard(totalScore);
        Debug.Log("Gửi điểm cuối cùng: " + totalScore);
    }
    private void ApplicationQuit()
    {
        if(totalScore > 0)
        {
            Debug.Log("Thoát game, gửi điểm lên hệ thống");
            SendFinalScore();
        }
    }
    public void SendScoreToLeaderboard(int score)
    {
        string mode = PlayerPrefs.GetString("LastMode", "Campaign"); 
        if(mode == "Campaign")
        {
            LeaderBoardCampaign.Instance.SendScoreCampaign(score);
            Debug.Log("Gửi điểm lên bxh Campign: "+score);
        }
        else if(mode == "Survive")
        {
            LeaderBoardSurvive.Instance.SendScoreSurvive(score);
            Debug.Log("Gửi điểm lên bxh Survive: " + score);
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SendFinalScore();
            ResetScore();
        }
    }
}
