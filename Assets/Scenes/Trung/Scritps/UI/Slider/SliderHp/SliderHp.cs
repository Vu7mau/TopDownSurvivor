using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SliderHp : SliderAbstract
{
    protected void FixedUpdate()
    {
        this.UpdateSlider();
    }
    protected virtual void UpdateSlider()
    {
        if (this.slider != null)
        {
            this.slider.value = this.GetValue();
        }

        if (this.sliderImageHP != null)
        {
            this.sliderImageHP.fillAmount = this.GetValue();
        }
    }
    protected abstract float GetValue();
}
