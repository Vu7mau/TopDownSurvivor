using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinemachineCtrl : VuMonoBehaviour
{
    public static CinemachineCtrl Instance;
    [Space]
    [Header("CinemachineCtrl")]
    [SerializeField] public CinemachineVirtualCamera _currentCinemachine;
    [SerializeField] public CinemachineVirtualCamera _defaultCinemachine;

    [SerializeField] protected CinemachineShake _cinemachineShake;
    public CinemachineShake CinemachineShake => _cinemachineShake;
    [SerializeField] protected CinemachineZoom _cinemachineZoom;
    public CinemachineZoom CinemachineZoom => _cinemachineZoom;


    [SerializeField] protected SwitchedCam _switchedCam;
    public SwitchedCam SwitchedCam => _switchedCam;

    protected override void Awake()
    {
        base.Awake();
        if(Instance == null)
            Instance = this;

        if(_defaultCinemachine!=null)
        {
            _currentCinemachine = _defaultCinemachine;
        }
    }
    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadCinemachineShake();
        this.LoadCinemachineVirtual();
        this.LoadCinemachineZoom();
        this.LoadSwitchedCam();
    }

    protected virtual void LoadCinemachineVirtual()
    {
        if (_defaultCinemachine != null) return;

        this._defaultCinemachine = GetComponent<CinemachineVirtualCamera>();
        Debug.Log("CinemachineVirtualCamera");

    }
    protected virtual void LoadCinemachineShake()
    {
        if( _currentCinemachine != null) return;

        this._cinemachineShake = transform.GetComponentInChildren<CinemachineShake>();
        Debug.Log("CinemachineVirtualCamera");
    } 
    protected virtual void LoadCinemachineZoom()
    {
        if(_cinemachineZoom != null) return;

        this._cinemachineZoom = transform.GetComponentInChildren<CinemachineZoom>();
        Debug.Log("CinemachineVirtualCamera");
    } 
    protected virtual void LoadSwitchedCam()
    {
        if(_switchedCam != null) return;

        this._switchedCam = transform.GetComponentInChildren<SwitchedCam>();
        Debug.Log("LoadSwitchedCam");
    }
}
