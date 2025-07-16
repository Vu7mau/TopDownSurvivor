using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class CircleWarning : EffectFX
{
    private static CircleWarning instance;
    public static CircleWarning Instance => instance;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadInstance();
        this.LoadTransform();
    }
    protected virtual void LoadInstance()
    {
        instance = this;
    }

    protected virtual void LoadTransform()
    {
        transform.localScale = new Vector3(1, 1, 1);
        transform.rotation = Quaternion.Euler(0,0,0);
    }
    public virtual void SetPosition(Vector3 position)
    {
        this.transform.position = position;
    }
}
