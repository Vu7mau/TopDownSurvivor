using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthBar1 : VuMonoBehaviour
{
    [SerializeField] protected float bossHP = 1000f;
    [SerializeField] protected List<Slider> hpSlider;

    [SerializeField] protected EnemyHealth enemyHealth;

    protected Slider healthSlider;
    protected float amountHPEachSlider;

    protected override void OnEnable()
    {
        base.OnEnable();
        this.LoadAllSlider();
        this.LoadEnemyHealth();
    }

    protected virtual void LoadAllSlider()
    {
        if(this.hpSlider.Count > 0)
        {
            foreach (Slider slider in this.hpSlider)
            {
                slider.maxValue = 1f;
                slider.minValue = 0f;
                slider.value = 1f;
            }
        }
        else
        {
            Debug.LogWarning("Chưa ref các thanh máu của boss mà bạn ei!");
        }
    }
    protected virtual void LoadEnemyHealth()
    {
        if (this.enemyHealth != null) return;
        this.enemyHealth = GetComponentInParent<EnemyHealth>();
    }

    protected virtual void GetBossHP(float _HP)
    {
        this.bossHP = _HP;
        this.amountHPEachSlider = GetAmountHPEachSlider();
    }
    protected virtual float GetAmountHPEachSlider() => (float)this.bossHP / this.hpSlider.Count;

    protected virtual void LoadHPSlider(Slider slider, float value)
    {
        this.healthSlider = slider;
    }

}
