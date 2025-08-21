using UnityEngine;
using UnityEngine.SceneManagement;
using System;
public class CountDownTimer : MonoBehaviour
{
    public static CountDownTimer Instance;
    public float elapsedTime = 0f;
    public bool isRunning = false;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    private void OnEnable()
    {
        StartTimer();
    }
    private void Update()
    {
        if (!isRunning) return;
        if (SceneManager.GetActiveScene().buildIndex == 1) return;
        elapsedTime += Time.deltaTime;
        GetFormattedTime();
    }

    public string GetFormattedTime()
    {
        int totalSeconds = Mathf.FloorToInt(elapsedTime);

        if (totalSeconds <= 0) return "0s";

        int days = totalSeconds / 86400;
        int hours = (totalSeconds % 86400) / 3600;
        int minutes = (totalSeconds % 3600) / 60;
        int seconds = totalSeconds % 60;

        string result = "";
        if (days > 0) result += $"{days}d ";
        if (hours > 0) result += $"{hours}h ";
        if (minutes > 0) result += $"{minutes}m ";
        if (seconds > 0) result += $"{seconds}s";

        return result.Trim();
    }

    public void StartTimer()
    {
        isRunning = true;
    }
    public void PauseTimer()
    {
        isRunning = false;
    }
    public void ResetTimer()
    {
        elapsedTime = 0f;
        isRunning = true;
        GetFormattedTime();
    }
    public int GetSessionDurationInSeconds()
    {
        return Mathf.FloorToInt(elapsedTime);
    }
}