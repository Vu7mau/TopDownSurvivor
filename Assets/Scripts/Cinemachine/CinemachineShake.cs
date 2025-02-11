using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CinemachineShake : VuMonoBehaviour
{
    public static CinemachineShake Instance { get; private set; }
    [SerializeField] protected CinemachineVirtualCamera cinemachineVirtualCamera;
    private float shakeTimer = 0;


    protected override void Awake()
    {
        if (Instance == null)
            Instance = this;
    }
    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadCamera();
    }
    protected virtual void LoadCamera()
    {
        if (cinemachineVirtualCamera == null) cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
    }

   public virtual void ShakeCamera(float intensity , float time)
    {
        CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin= cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
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

                CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
                cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0;
            }
        }
        
    }
}
