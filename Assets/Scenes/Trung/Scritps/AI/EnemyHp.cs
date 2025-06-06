using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHp : SliderHp
{
    [SerializeField] protected EnemyHealth enemyHealth;

    protected override void UpdateSlider()
    {
        base.UpdateSlider();
        this.HideTheHealthBar();
    }
    protected virtual void HideTheHealthBar()
    {
        if(this.GetValue() <= 0) this.slider.transform.parent.gameObject.SetActive(false);
    }
    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadEnemyHealth();
    }

    protected virtual void LoadEnemyHealth()
    {
        if (enemyHealth != null) return;
        this.enemyHealth = GetComponentInParent<EnemyHealth>();
        Debug.Log(transform.name + ": Load EnemyHealth"/*,gameObject*/);
    }
    protected override float GetValue() => (float)enemyHealth.Health / (float)enemyHealth.MaxHealth;
}
