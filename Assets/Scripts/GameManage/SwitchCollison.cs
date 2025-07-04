using System.Collections;
using System.Collections.Generic;
using Hanzzz.MeshDemolisher;
using UnityEngine;

public class SwitchCollison : VuMonoBehaviour
{
    [SerializeField] private CameraType cameraTypeEnter;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            CinemachineCtrl.Instance.SwitchedCam.SetCameraType(cameraTypeEnter);
            Debug.Log("hee");
        }
        
    }
}
