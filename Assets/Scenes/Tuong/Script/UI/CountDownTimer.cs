using UnityEngine;
using UnityEngine.SceneManagement;
using System;
public class CountDownTimer : MonoBehaviour
{
    public static CountDownTimer Instance;
    public float elapsedTime = 0f;
    private bool isRunning = false;
    private DateTime sessionStartTime;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        sessionStartTime = DateTime.UtcNow;
        StartTimer();
    }

    private void Update()
    {
        if (!isRunning) return;
        if (SceneManager.GetActiveScene().buildIndex == 1) return;
        elapsedTime += Time.deltaTime;
        UpdateTimerUI();
    }

    private void UpdateTimerUI()
    {
        int totalSeconds = Mathf.FloorToInt(elapsedTime);

        int days = totalSeconds / 86400;           
        int hours = (totalSeconds % 86400) / 3600;
        int minutes = (totalSeconds % 3600) / 60;
        int seconds = totalSeconds % 60;

        string display;

        if (days > 0)
        {
            display = $"{days}d {hours:D2}:{minutes:D2}:{seconds:D2}";
        }
        else
        {
            display = $"{hours:D2}:{minutes:D2}:{seconds:D2}";
        }

        Debug.Log(display);
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
        isRunning = false;
        UpdateTimerUI();
    }
    public int GetSessionDurationInSeconds()
    {
        return Mathf.FloorToInt(elapsedTime);
    }
}