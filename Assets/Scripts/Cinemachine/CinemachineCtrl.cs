using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinemachineCtrl : VuMonoBehaviour
{
    public static CinemachineCtrl Instance;
    [Space]
    [Header("CinemachineCtrl")]
    [SerializeField] public CinemachineVirtualCamera _cinemachineVirtualCamera;

    [SerializeField] protected CinemachineShake _cinemachineShake;
    public CinemachineShake CinemachineShake => _cinemachineShake;
    [SerializeField] protected CinemachineZoom _cinemachineZoom;
    public CinemachineZoom CinemachineZoom => _cinemachineZoom;

    protected override void Awake()
    {
        base.Awake();
        if(Instance == null)
            Instance = this;
    }
    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadCinemachineShake();
        this.LoadCinemachineVirtual();
        this.LoadCinemachineZoom();
    }

    protected virtual void LoadCinemachineVirtual()
    {
        if (_cinemachineVirtualCamera != null) return;

        this._cinemachineVirtualCamera= GetComponent<CinemachineVirtualCamera>();
        Debug.Log("CinemachineVirtualCamera");

    }
    protected virtual void LoadCinemachineShake()
    {
        if( _cinemachineVirtualCamera != null) return;

        this._cinemachineShake = transform.GetComponentInChildren<CinemachineShake>();
        Debug.Log("CinemachineVirtualCamera");
    } 
    protected virtual void LoadCinemachineZoom()
    {
        if(_cinemachineZoom != null) return;

        this._cinemachineZoom = transform.GetComponentInChildren<CinemachineZoom>();
        Debug.Log("CinemachineVirtualCamera");
    }
}
