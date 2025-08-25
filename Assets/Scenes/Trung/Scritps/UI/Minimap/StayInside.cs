using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class StayInside : VuMonoBehaviour
{
    public Transform MinimapCam;
    public float MinimapSize = 10;
    Vector3 TempV3;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadMinimap();
    }

    protected virtual void LoadMinimap()
    {
        this.MinimapCam = FindAnyObjectByType<MiniMap>().transform;
    }


    protected void Update()
    {
        TempV3 = transform.parent.transform.position;
        TempV3.y = transform.position.y;
        transform.position = TempV3;
    }


    protected void LateUpdate()
    {
        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, MinimapCam.position.x - MinimapSize, MinimapSize + MinimapCam.position.x),
            transform.position.y,
            Mathf.Clamp(transform.position.z, MinimapCam.position.z - MinimapSize, MinimapSize + MinimapCam.position.z)
        );
    }

}
