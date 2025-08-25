using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class Timer : MonoBehaviour
{
    [SerializeField] protected TMP_Text timerText;   // Text hiển thị thời gian
    [SerializeField] protected bool isCountdown = false; // true = đếm ngược, false = đếm tiến
    [SerializeField] protected float startTime = 60f;    // Thời gian bắt đầu (chỉ dùng cho đếm ngược)

    private float currentTime;
    private bool isRunning = false;

    private bool timeIsUp = false;
    public bool TimeIsUp { get => this.timeIsUp; }

    void Start()
    {
        
    }

    protected virtual void Update()
    {
        if (!isRunning) return;

        if (isCountdown)
        {
            currentTime -= Time.deltaTime;
            if (currentTime <= 0f)
            {
                currentTime = 0f;
                this.timeIsUp = true;
                StopTimer(); // Dừng khi hết giờ
            }
        }
        else
        {
            currentTime += Time.deltaTime;
        }

        this.UpdateTimerUI();
    }

    public virtual void SetCurrentTime()
    {
        
    }

    public virtual void StartCount(bool isCountdown , float time)
    {
        this.timeIsUp = !isCountdown;
        this.startTime = time;
        this.isRunning = isCountdown;
        this.isCountdown = isCountdown;
        this.ResetTimer();
    }

    void UpdateTimerUI()
    {
        int minutes = Mathf.FloorToInt(currentTime / 60f);
        int seconds = Mathf.FloorToInt(currentTime % 60f);
        this.timerText.text = $"{minutes:00}:{seconds:00}";
    }

    public void StartTimer() => isRunning = true;
    public void StopTimer() => isRunning = false;
    public void ResetTimer()
    {
        currentTime = isCountdown ? startTime : 0f;
        this.UpdateTimerUI();
    }

    public void ToggleTimer() => isRunning = !isRunning;
    private void SwitchStateCount()
    {
        
    }
}
