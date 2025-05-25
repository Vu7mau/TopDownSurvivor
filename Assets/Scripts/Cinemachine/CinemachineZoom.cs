using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinemachineZoom : CinemachineAbstract
{
    [SerializeField] private float _defaultFOV;
    [SerializeField] protected float _zoomInFOV;
    [SerializeField] protected float _zoomSpeed;
  //  [SerializeField] protected bool _isZoom;
    
    //private void LateUpdate()
    //{
    //    this.ToggleZoom();
    //}
    //public void SetIsZoom(bool isZoom)
    //{
    //    this._isZoom = isZoom;
    //}
    public virtual void ToggleZoom(bool isZoom)
    {
        if (isZoom)
            this.ZoomIn();
        else
            this.ZoomOut();
    }    
    protected virtual void ZoomIn()
    {
        var lensSettings = _cinemachineCtrl._cinemachineVirtualCamera.m_Lens;
        lensSettings.OrthographicSize= Mathf.Lerp(lensSettings.OrthographicSize, _zoomInFOV, _zoomSpeed*Time.deltaTime);
        _cinemachineCtrl._cinemachineVirtualCamera.m_Lens.OrthographicSize = lensSettings.OrthographicSize;
    }
    protected virtual void ZoomOut()
    {
        var lensSettings = _cinemachineCtrl._cinemachineVirtualCamera.m_Lens;
        lensSettings.OrthographicSize = Mathf.Lerp(lensSettings.OrthographicSize, _defaultFOV, _zoomSpeed*Time.deltaTime);
        _cinemachineCtrl._cinemachineVirtualCamera.m_Lens.OrthographicSize = lensSettings.OrthographicSize;
    }
}
