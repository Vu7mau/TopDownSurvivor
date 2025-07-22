using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class SwitchedCam : CinemachineAbstract
{
    // public CameraType cameraType;
    public static Action<CinemachineVirtualCamera> OnCameraSwitched { get; private set; }
    protected override void OnEnable()
    {
        OnCameraSwitched += SwitchCam;
    }
    protected override void OnDisable()
    {
        OnCameraSwitched -= SwitchCam;
    }
    public void SetCameraType(CameraType type)
    {
        if (_cinemachineCtrl._currentCinemachine == null) return;

        switch (type)
        {
            case CameraType.Perspective:
                _cinemachineCtrl._currentCinemachine.m_Lens.ModeOverride = LensSettings.OverrideModes.Perspective;
                Camera.main.ResetProjectionMatrix();
                break;
            case CameraType.Orthographic:
                _cinemachineCtrl._currentCinemachine.m_Lens.ModeOverride = LensSettings.OverrideModes.Orthographic;
                Camera.main.ResetProjectionMatrix();
                break;
            case CameraType.Physical:
                _cinemachineCtrl._currentCinemachine.m_Lens.ModeOverride = LensSettings.OverrideModes.Physical;
                Camera.main.ResetProjectionMatrix();
                break;
        }
    }
    private void SwitchCam(CinemachineVirtualCamera cam)
    {
        if (cam == null) return;

        cam.Priority = _cinemachineCtrl._currentCinemachine.Priority + 1;
       
        _cinemachineCtrl._currentCinemachine.Priority = _cinemachineCtrl._currentCinemachine.Priority - 1;
        _cinemachineCtrl._currentCinemachine=cam;
    }

}
public enum CameraType
{
    Perspective = 1,
    Orthographic = 2,
    Physical = 3
}