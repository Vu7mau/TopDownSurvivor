using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScoreManager : MonoBehaviour
{
    public static PlayerScoreManager Instance;
    public int currentScore = 0;
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else Destroy(gameObject);
    }
    public void AddScore(int amount)
    {
        currentScore += amount;
        Debug.Log("Điểm hiện tại: " + currentScore);
    }
    public int GetCurrentScore()
    {
        return currentScore;
    }
    public void ResetScore()
    {
        currentScore = 0;
    }
}
