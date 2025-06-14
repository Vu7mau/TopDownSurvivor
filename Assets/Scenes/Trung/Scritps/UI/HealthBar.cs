using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : VuMonoBehaviour
{
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Slider healthEaseSlider;
    [SerializeField] protected HpBarObj hpBarObj;
    [SerializeField] protected EnemyHealth enemyHealth;
    [SerializeField] private float timeEaseLerp = 0.05f;
    protected override void OnEnable()
    {
        base.OnEnable();
        this.ResetHealthSlider();
    }
    protected override void OnDisable()
    {
        base.OnDisable();
    }
    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadEnemyHealth();
        this.LoadSlider();
        this.LoadHpBarObj();
    }
    protected void Update()
    {
        this.SetUpValueForSliders();
        if (this.healthEaseSlider.value != this.healthSlider.value)
            this.healthEaseSlider.value = Mathf.Lerp(this.healthEaseSlider.value, this.healthSlider.value, this.timeEaseLerp);
    }
    protected virtual void UpdateHealthBar()
    {
        if(this.enemyHealth.Health <= 0) { this.hpBarObj.gameObject.SetActive(false); return; }
        if (this.healthSlider == null || this.healthEaseSlider == null) { this.LoadSlider(); }
    }
    protected virtual void SetUpValueForSliders()
    {
        if (this.healthSlider == null || this.healthEaseSlider == null) { this.LoadSlider(); }
        this.healthSlider.value = (float)this.enemyHealth.Health / (float)this.enemyHealth.MaxHealth;
        //this.healthSlider.maxValue = this.enemyHealth.MaxHealth;
        //this.healthEaseSlider.maxValue = this.enemyHealth.MaxHealth;
        this.UpdateHealthBar();
    }
    protected virtual void LoadSlider()
    {
        this.healthSlider = GetComponentInChildren<HealthBarTemp>().GetComponent<Slider>();
        this.healthEaseSlider = GetComponentInChildren<HealthEaseBarTemp>().GetComponent<Slider>();
    }
    protected virtual void LoadEnemyHealth()
    {
        if(this.enemyHealth != null) return;
        this.enemyHealth = GetComponentInParent<EnemyHealth>();
        Debug.Log(transform.name + ": Load EnemyHealth", gameObject);
    }
    protected virtual void LoadHpBarObj()
    {
        if (this.hpBarObj != null) return;
        this.hpBarObj = GetComponentInParent<HpBarObj>();
        Debug.Log(transform.name + ": Load EnemyHealth", gameObject);
    }
    protected virtual void ResetHealthSlider()
    {
        this.healthSlider.value = this.healthSlider.maxValue;
        this.healthEaseSlider.value = this.healthEaseSlider.maxValue;
    }
}
