using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthBar : MonoBehaviour
{
    [SerializeField] private Slider bossHealthBars;
    [SerializeField] private float health;
    [SerializeField] private float ease;
    private float maxHealth;

    private void Start()
    {
        StartCoroutine(HideBarRoutine());
    }
    private void Update()
    {
        UpdateBossHealthBar(health);
    }
    private IEnumerator HideBarRoutine()
    {
        yield return new WaitUntil(() => health == 0);
        Debug.Log("Boss đã chết!");
        gameObject.SetActive(false);
        yield return null;
    }
    public void UpdateCurrentHealthBoss(float _currentHealthEachBar)
    {
        health = _currentHealthEachBar;
    }
    public void UpdateBossHealthBar(float _currentHealthEachBar)
    {
        if (bossHealthBars.value != _currentHealthEachBar)
            bossHealthBars.value = Mathf.Lerp(bossHealthBars.value, _currentHealthEachBar, ease);
    }
    public void SetUpBar(float _maxHealth)
    {
        health = _maxHealth;
        bossHealthBars.maxValue = _maxHealth;
        bossHealthBars.value = _maxHealth;
    }
}
