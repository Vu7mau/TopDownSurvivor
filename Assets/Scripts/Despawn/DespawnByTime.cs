using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DespawnByTime : Despawn
{


    [Header("Despawn By Time")]
    [SerializeField] protected float _timeDelay = 2;
    [SerializeField] protected float _timer = 0;
  
    protected override bool CanDespawn()
    {
        this._timer += Time.fixedDeltaTime;
        if(this._timer > this._timeDelay)
        {
            _timer = 0;
            return true;
        }
        return false;
    }

}
