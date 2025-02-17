using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CinemachineAbstract : VuMonoBehaviour
{
    [Space]
    [Header("CinemachineCAbstract")]
    [SerializeField] protected CinemachineCtrl _cinemachineCtrl;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadCinemachineCtrl();
    }
    protected virtual void LoadCinemachineCtrl()
    {
        if (_cinemachineCtrl != null) return;

        this._cinemachineCtrl=this.transform.parent.GetComponent<CinemachineCtrl>();
        Debug.Log("LoadCinemachineCtrl");
    }
}
