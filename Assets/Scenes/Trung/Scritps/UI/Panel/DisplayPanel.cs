using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class DisplayPanel : Panel
{
    public override void AnimationDisplayOnFX()
    {
        if (!this.isEaseOn) return;
        RectTransform rectObj = this.GetComponent<RectTransform>();
        rectObj.localScale = Vector3.zero;
        rectObj.DOScale(Vector3.one, this.duration).SetEase(this.animEaseOn).SetUpdate(true);
    }

    public override void AnimationDisplayOffFX()
    {
        
    }
}
