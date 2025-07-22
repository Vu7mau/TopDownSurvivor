using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraSwitchPointTrigger : VuMonoBehaviour
{
    [SerializeField] public CinemachineVirtualCamera cam;


    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadCinemchine();
    }
    private void LoadCinemchine()
    {
        if(cam != null) return;

        cam=this.transform.GetComponentInChildren<CinemachineVirtualCamera>();
    }    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            SwitchedCam.OnCameraSwitched(cam);
        }
    }
}
