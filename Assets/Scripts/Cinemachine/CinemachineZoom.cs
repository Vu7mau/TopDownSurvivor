using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinemachineZoom : CinemachineAbstract
{
    [SerializeField] private float _defaultFOV;
    [SerializeField] protected float _zoomInFOV;
   // [SerializeField] protected float _zoomSpeed;
  //  [SerializeField] protected bool _isZoom;
    
    //private void LateUpdate()
    //{
    //    this.ToggleZoom();
    //}
    //public void SetIsZoom(bool isZoom)
    //{
    //    this._isZoom = isZoom;
    //}
    public virtual void ToggleZoom(bool isZoom,float zoomSpeed)
    {
        if (isZoom)
            this.ZoomIn(zoomSpeed);
        else
            this.ZoomOut(zoomSpeed);
    }    
    protected virtual void ZoomIn(float zoomSpeed)
    {
        var lensSettings = _cinemachineCtrl._cinemachineVirtualCamera.m_Lens;
        lensSettings.OrthographicSize= Mathf.Lerp(lensSettings.OrthographicSize, _zoomInFOV, zoomSpeed * Time.deltaTime);
        _cinemachineCtrl._cinemachineVirtualCamera.m_Lens.OrthographicSize = lensSettings.OrthographicSize;
    }
    protected virtual void ZoomOut(float zoomSpeed)
    {
        var lensSettings = _cinemachineCtrl._cinemachineVirtualCamera.m_Lens;
        lensSettings.OrthographicSize = Mathf.Lerp(lensSettings.OrthographicSize, _defaultFOV, zoomSpeed * Time.deltaTime);
        _cinemachineCtrl._cinemachineVirtualCamera.m_Lens.OrthographicSize = lensSettings.OrthographicSize;
    }
}
