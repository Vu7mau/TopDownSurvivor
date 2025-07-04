using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class SwitchedCam : CinemachineAbstract
{
   // public CameraType cameraType;

    public void SetCameraType(CameraType type)
    {
        if (_cinemachineCtrl._cinemachineVirtualCamera == null) return;

        switch (type)
        {
            case CameraType.Perspective:
                _cinemachineCtrl._cinemachineVirtualCamera.m_Lens.ModeOverride = LensSettings.OverrideModes.Perspective;
                Camera.main.ResetProjectionMatrix();
                break;
            case CameraType.Orthographic:
                _cinemachineCtrl._cinemachineVirtualCamera.m_Lens.ModeOverride = LensSettings.OverrideModes.Orthographic;
                Camera.main.ResetProjectionMatrix();
                break;
            case CameraType.Physical:
                _cinemachineCtrl._cinemachineVirtualCamera.m_Lens.ModeOverride = LensSettings.OverrideModes.Physical;
                Camera.main.ResetProjectionMatrix();
                break;
        }
    }

}
public enum CameraType
{
    Perspective=1,
    Orthographic=2,
    Physical=3
}