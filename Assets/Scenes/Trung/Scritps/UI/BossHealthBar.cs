using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthBar : SliderHp
{
    [SerializeField] private EnemyHealth enemyHealth;
    [SerializeField] private HpBarObj hpBarObj;
    //[SerializeField] private float health;
    [SerializeField] private float ease;
    private float maxHealth;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadEnemyHealth();
        this.LoadHpBarObj();
    }
    private void Update()
    {
        this.HideTheHealthBar();
    }
    private void HideTheHealthBar()
    {
        if (this.enemyHealth.Health > 0) return;
        this.hpBarObj.gameObject.SetActive(false);
    }

    protected virtual void LoadEnemyHealth()
    {
        if (this.enemyHealth != null) return;
        this.enemyHealth = GetComponentInParent<EnemyHealth>();
    }
    protected virtual void LoadHpBarObj()
    {
        if (this.hpBarObj != null) return;
        this.hpBarObj = GetComponentInParent<HpBarObj>();
    }

    //private void Start()
    //{
    //    StartCoroutine(HideBarRoutine());
    //}
    //private void Update()
    //{
    //    UpdateBossHealthBar(health);
    //}
    //private IEnumerator HideBarRoutine()
    //{
    //    yield return new WaitUntil(() => health == 0);
    //    Debug.Log("Boss đã chết!");
    //    gameObject.SetActive(false);
    //    yield return null;
    //}
    protected override float GetValue()
    {
        return (float)enemyHealth.Health / (float)enemyHealth.MaxHealth;
    }
    
}
