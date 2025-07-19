using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthBar : SliderHp
{
    [SerializeField] private EnemyHealth enemyHealth;


    [SerializeField] private HpBarObj hpBarObj;
    [SerializeField] protected Slider healthBarEase;
    [SerializeField] protected TextMeshProUGUI txtHealthProgress;
    [SerializeField] protected TextMeshProUGUI txtBossName;


    //[SerializeField] private float health;
    [SerializeField] private float ease;
    private float maxHealth;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadEnemyHealth();
        this.LoadHpBarObj();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        StartCoroutine(ResetHealthSliderRoutine());
    }
    protected override void OnDisable()
    {
        base.OnDisable();
    }
    private void Update()
    {
        this.UpdateHealthTitle();
        this.UpdateHealthEaseBar();
        this.HideTheHealthBar();
    }
    private void HideTheHealthBar()
    {
        if (this.enemyHealth.Health > 0) return;
        this.hpBarObj.gameObject.SetActive(false);
    }

    protected IEnumerator ResetHealthSliderRoutine()
    {
        yield return new WaitForSeconds(0.1f);
        this.ResetHealthSlider();
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

    protected virtual void UpdateHealthTitle()
    {
        if(this.txtBossName != null)
        {
            //txtBossName.text = this.enemyCtrl.BOSS_ROBOT_SCIFI;
        }
        if(this.txtHealthProgress != null)
        {
            float pro = GetValue() * 100;
            this.txtHealthProgress.text = pro.ToString() + "%";
        }
    }
    protected override float GetValue()
    {
        return (float)enemyHealth.Health / (float)enemyHealth.MaxHealth;
    }
    protected void UpdateHealthEaseBar()
    {
        if (this.healthBarEase.value != this.slider.value)
        {
            this.healthBarEase.value = Mathf.Lerp(this.healthBarEase.value, this.slider.value, ease);
        }
    }
    protected virtual void UpdateHealthBar()
    {
       // if (this.enemyHealth.Health <= 0) { this.hpBarObj.gameObject.SetActive(false); return; }
        //if (this.slider == null || this.healthBarEase == null) { this.LoadSlider(); }
    }
    protected virtual void SetUpValueForSliders()
    {
        //if (this.slider == null || this.healthBarEase == null) { this.LoadSlider(); }
        //this.slider.value = GetValue();
        ////this.healthSlider.maxValue = this.enemyHealth.MaxHealth;
        ////this.healthEaseSlider.maxValue = this.enemyHealth.MaxHealth;
        //this.UpdateHealthBar();
    }
    protected virtual void ResetHealthSlider()
    {
        this.slider.value = this.slider.maxValue;
        this.healthBarEase.value = this.healthBarEase.maxValue;
    }
}
