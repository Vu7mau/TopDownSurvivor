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
        if (SceneManager.GetActiveScene().buildIndex == 0) return;
        elapsedTime += Time.deltaTime;
        UpdateTimerUI();
    }

    private void UpdateTimerUI()
    {
        int totalSeconds = Mathf.FloorToInt(elapsedTime);
        int h = totalSeconds / 3600;
        int m = (totalSeconds % 3600) / 60;
        int s = totalSeconds % 60;
        //Debug.Log(string.Format("{0:D2}:{1:D2}:{2:D2}", h, m, s));
    }
    public void StartTimer()
    {
        isRunning = true;
        sessionStartTime = DateTime.UtcNow;
    }
    public void PauseTimer()
    {
        isRunning = false;
    }
    public void ResetTimer()
    {
        elapsedTime = 0f;
        isRunning = false;
        sessionStartTime = DateTime.UtcNow;
        UpdateTimerUI();
    }
    public int GetSessionDurationInSeconds()
    {
        return (int)(DateTime.UtcNow - sessionStartTime).TotalSeconds;
    }
}