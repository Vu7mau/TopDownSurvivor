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
    private void Start()
    {
        healthEaseSlider.maxValue = maxHealth;
        healthEaseSlider.value = maxHealth;
        healthSlider.maxValue = maxHealth;
        health = maxHealth;
        healthSlider.value = health;
    }
    private void Update()
    {
        if(healthSlider.value != health)
        {
            healthSlider.value = health;
        }
        if(Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(30);
        }
        if(healthEaseSlider.value != healthSlider.value)
        {
            healthEaseSlider.value = Mathf.Lerp(healthEaseSlider.value,health, timeEaseLerp);
        }
    }
    private void TakeDamage(float damage)
    {
        health-=damage;
    }
}
