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
    [SerializeField] private bool isCountUp;

    private void Awake()
    {
        Instance = this;
    }
    private void Update()
    {
        SwitchStateCount();
        CountDown(startCountTime,isCountDown);
        CountUp(startCountTime, isCountUp);
    }

    //Bắt đầu tính giờ
    public void StartCountDown(bool _isStartCountTime, bool _isCountDown,float _time)
    {
        time = _time;
        CountDown(_isStartCountTime,_isCountDown);
    }
    public void StartCountUp(bool _isStartCountTime, bool _isCountUp, float _time)
    {
        time = _time;
        CountUp(_isStartCountTime, _isCountUp);
    }

    //Dừng tính giờ
    public void StopCountDown(bool _isStopCountTime, bool _isCountDown)
    {
        CountDown(_isStopCountTime, _isCountDown);
    }
    public void StopCountUp(bool _isStopCountTime, bool _isCountUp)
    {
        CountUp(_isStopCountTime, _isCountUp);
    }

    //Tiếp tục tính giờ
    public void ContinueCountDown(bool _isContinueCountTime, bool _isCountDown)
    {
        CountDown(_isContinueCountTime, _isCountDown);
    }
    public void ContinueCountUp(bool _isContinueCountTime, bool _isCountUp)
    {
        CountUp(_isContinueCountTime, _isCountUp);
    }

    public void CountDown(bool _isStartCountTime,bool _isCountDown)
    {
        startCountTime = _isStartCountTime;
        isCountDown = _isCountDown;
        UpdateTimeCount();
        if(!startCountTime)  return;
        if (!isCountDown) return;
        if(time > 0)
            time -= Time.deltaTime;
        else
        {
            time = 0;
            if (SpawnEnemies.Instance.WaveNumber > SpawnEnemies.Instance.AmountWave)
            {
                SpawnEnemies.Instance.FinishTheBattle(true);
            }
        }
    }
    public void CountUp(bool _isStartCountTime, bool _isCountUp)
    {
        startCountTime = _isStartCountTime;
        isCountUp = _isCountUp;
        UpdateTimeCount();
        if (!startCountTime) return;
        if (!isCountUp) return;
        if (time > 0)
            time += Time.deltaTime;
        else
            time = 0;
    }
    private void UpdateTimeCount()
    {
        int miniutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        txtTime.text = string.Format("{0:00}:{1:00}", miniutes, seconds);
    }
    private void SwitchStateCount()
    {
        
    }
}
