using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : VuMonoBehaviour
{
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Slider healthEaseSlider;
    [SerializeField] protected EnemyHealth enemyHealth;
    [SerializeField] private float timeEaseLerp = 0.05f;
    protected override void OnEnable()
    {
        base.OnEnable();
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
    }
    protected void Update()
    {
        this.SetUpValueForSliders();
        if (this.healthEaseSlider.value != this.healthSlider.value)
            this.healthEaseSlider.value = Mathf.Lerp(this.healthEaseSlider.value, this.enemyHealth.Health, this.timeEaseLerp);
    }
    protected virtual void UpdateHealthBar()
    {
        if(this.enemyHealth.Health <= 0) { this.gameObject.SetActive(false); return; }
        if (this.healthSlider == null || this.healthEaseSlider == null) { this.LoadSlider(); }
    }
    protected virtual void SetUpValueForSliders()
    {
        if (this.healthSlider == null || this.healthEaseSlider == null) { this.LoadSlider(); }
        this.healthSlider.value = this.enemyHealth.Health;
        this.healthSlider.maxValue = this.enemyHealth.MaxHealth;
        this.healthEaseSlider.maxValue = this.enemyHealth.MaxHealth;
        this.UpdateHealthBar();
    }
    protected virtual void LoadSlider()
    {
        this.healthSlider = GameObject.Find("HealthBar").GetComponent<Slider>();
        this.healthEaseSlider = GameObject.Find("HealthEaseBar").GetComponent<Slider>();
    }
    protected virtual void LoadEnemyHealth()
    {
        if(this.enemyHealth != null) return;
        this.enemyHealth = GetComponentInParent<EnemyHealth>();
        Debug.Log(transform.name + ": Load EnemyHealth", gameObject);
    }
}
