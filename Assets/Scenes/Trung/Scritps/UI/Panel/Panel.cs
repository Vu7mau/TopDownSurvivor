using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public abstract class Panel : VuMonoBehaviour
{
    [Header("Animation When Display")]
    [SerializeField] protected float duration = 0.5f;
    [SerializeField] protected Ease animEaseOn = Ease.OutBack;
    [SerializeField] protected bool isEaseOn = true;
    //[SerializeField] protected bool isEaseOff = true;
    //[SerializeField] protected Ease animEaseOff = Ease.OutBack;

    public abstract void AnimationDisplayOnFX();
    public abstract void AnimationDisplayOffFX();

    protected override void OnEnable()
    {
        base.OnEnable();
        this.AnimationDisplayOnFX();
    }
}
