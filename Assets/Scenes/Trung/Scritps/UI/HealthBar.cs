using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Slider healthEaseSlider;
    [SerializeField] private float maxHealth;
    private float health;
    [SerializeField] private float timeEaseLerp = 0.05f;
    private void OnDisable()
    {
        LoadHealthBar((int)maxHealth);
    }
    private void Update()
    {
        if (healthSlider.value != health) healthSlider.value = health;
        if (healthEaseSlider.value != healthSlider.value)
            healthEaseSlider.value = Mathf.Lerp(healthEaseSlider.value, health, timeEaseLerp);
    }
    public void LoadHealthBar(int currentHealth)
    {
        health = currentHealth;
        UpdateHealthBar();
    }
    private void UpdateHealthBar()
    {
        if(health <= 0) { gameObject.SetActive(false); return; }
        if (this.healthSlider == null || this.healthEaseSlider == null) { this.LoadSlider(); }
    }
    private void SetUpValueForSliders()
    {
        if (this.healthSlider == null || this.healthEaseSlider == null) { this.LoadSlider(); }
        this.healthEaseSlider.maxValue = health;
        this.healthSlider.maxValue = health;
        this.healthSlider.value = health;
    }
    public void LoadMaxHealth(int _maxHealth)
    {
        this.maxHealth = _maxHealth;
        this.health = maxHealth;
        SetUpValueForSliders();
    }
    private void LoadSlider()
    {
        this.healthSlider = GameObject.Find("HealthBar").GetComponent<Slider>();
        this.healthEaseSlider = GameObject.Find("HealthEaseBar").GetComponent<Slider>();
    }
}
