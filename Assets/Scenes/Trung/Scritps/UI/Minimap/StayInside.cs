using System.Collections;
using System.Collections.Generic;
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
        var miniMapObj = FindAnyObjectByType<MiniMap>();
        if (miniMapObj == null) return;
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
        if(this.MinimapCam == null) return;
        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, MinimapCam.position.x - MinimapSize, MinimapSize + MinimapCam.position.x),
            transform.position.y,
            Mathf.Clamp(transform.position.z, MinimapCam.position.z - MinimapSize, MinimapSize + MinimapCam.position.z)
        );
    }

}
