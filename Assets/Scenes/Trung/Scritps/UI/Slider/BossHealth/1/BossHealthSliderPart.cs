using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealthSliderPart : SliderHp
{
    [SerializeField] protected float _hpMax;
    public float HPMax { get => this._hpMax; set => this._hpMax = value; }

    [SerializeField] protected float _hp;
    public float HP { get => this._hp; set => this._hp = value; }

    protected override float GetValue()
    {
        return (float )this._hp / (float)this._hpMax;
    }
}
