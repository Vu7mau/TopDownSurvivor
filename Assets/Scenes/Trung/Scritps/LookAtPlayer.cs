using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtPlayer : VuMonoBehaviour
{
    [SerializeField] private Transform playerCamera;
    [SerializeField] private Transform targetPosition;
    [SerializeField] private Vector3 offSet;
    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadCamera();
        this.LoadTargetPosition();
    }
    protected virtual void LoadCamera()
    {
        if (this.playerCamera != null) return;
        this.playerCamera = GameObject.Find("Main Camera").transform;
        Debug.Log(transform.name + "Load Camera", gameObject);
    }
    protected virtual void LoadTargetPosition()
    {
        if (this.targetPosition != null) return;
        this.targetPosition = transform.parent;
        Debug.Log(transform.name + "Load Target", gameObject);
    }
    private void Update()
    {
        transform.rotation = playerCamera.transform.rotation;
        transform.position = targetPosition.position + offSet;
    }
}
