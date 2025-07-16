using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EffectFX : PoolObj
{
    public virtual void Transform(Vector3 position)
    {
        this.transform.position = position;
    }
    public virtual void Scale(Vector3 scale)
    {
        this.transform.localScale = scale;
    }
    public virtual void Rotate(Vector3 rotation)
    {
        this.transform.rotation = Quaternion.Euler(rotation);
    }
}
