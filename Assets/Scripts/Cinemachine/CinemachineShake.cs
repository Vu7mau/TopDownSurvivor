using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CinemachineShake : CinemachineAbstract
{

    [Space]
    [Header("CinemachineShake")]
    private float shakeTimer = 0;

    CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin;
    protected override void Awake()
    {
        
    }
    protected override void LoadComponents()
    {
        base.LoadComponents();
    }

   public virtual void ShakeCamera(float intensity , float time)
    {
        cinemachineBasicMultiChannelPerlin = _cinemachineCtrl._cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
       cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = intensity;
        shakeTimer=time;
    }

    private void Update()
    {
        if (shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;
            if (shakeTimer <= 0)
            {
              
                cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0;
            }
        }
        
    }
}
