using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{

    public static Timer Instance;
    [SerializeField] private TextMeshProUGUI txtTime;
    [SerializeField] private float time;

    [SerializeField] private bool startCountTime = true;
    [SerializeField] private bool isCountDown;
    [SerializeField] private bool timeIsUp = false;
    public bool TimeIsUp { get => this.timeIsUp; }
    [SerializeField] private bool isCountUp;

    private void Awake()
    {
        Instance = this;
    }
    private void Update()
    {
        this.SwitchStateCount();
        this.CountDown(startCountTime,isCountDown);
        this.CountUp(startCountTime, isCountUp);
    }

    //Bắt đầu tính giờ
    public void StartCountDown(bool _isStartCountTime, bool _isCountDown,float _time)
    {
        this.timeIsUp = false;
        this.time = _time;
        this.CountDown(_isStartCountTime,_isCountDown);
    }
    public void StartCountUp(bool _isStartCountTime, bool _isCountUp, float _time)
    {
        this.timeIsUp = false;
        this.time = _time;
        this.CountUp(_isStartCountTime, _isCountUp);
    }

    //Dừng tính giờ
    public void StopCountDown(bool _isStopCountTime, bool _isCountDown)
    {
        this.CountDown(_isStopCountTime, _isCountDown);
    }
    public void StopCountUp(bool _isStopCountTime, bool _isCountUp)
    {
        this.CountUp(_isStopCountTime, _isCountUp);
    }

    //Tiếp tục tính giờ
    public void ContinueCountDown(bool _isContinueCountTime, bool _isCountDown)
    {
        this.CountDown(_isContinueCountTime, _isCountDown);
    }
    public void ContinueCountUp(bool _isContinueCountTime, bool _isCountUp)
    {
        this.CountUp(_isContinueCountTime, _isCountUp);
    }

    public void CountDown(bool _isStartCountTime,bool _isCountDown)
    {
        this.startCountTime = _isStartCountTime;
        this.isCountDown = _isCountDown;
        this.UpdateTimeCount();
        if(!this.startCountTime)  return;
        if (!this.isCountDown) return;
        if(this.time > 0)
            this.time -= Time.deltaTime;
        else
        {
            this.time = 0;
            this.timeIsUp = true;
            return;
        }
    }
    public void CountUp(bool _isStartCountTime, bool _isCountUp)
    {
        this.startCountTime = _isStartCountTime;
        this.isCountUp = _isCountUp;
        this.UpdateTimeCount();
        if (!this.startCountTime) return;
        if (!this.isCountUp) return;
        if (this.time > 0)
            this.time += Time.deltaTime;
        else
            this.time = 0;
    }
    private void UpdateTimeCount()
    {
        int miniutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        this.txtTime.text = string.Format("{0:00}:{1:00}", miniutes, seconds);
    }
    private void SwitchStateCount()
    {
        
    }
}
