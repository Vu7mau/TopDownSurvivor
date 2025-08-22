using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStatsUI : VuMonoBehaviour
{
    [SerializeField] protected CharacterStats characterStats;
    [SerializeField] protected CharacterCtrl characterCrtl;

    protected DamageSender damageSender;



    [SerializeField] protected bool isOnStats = false;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadCharacterStats();
        this.LoadCharacterCtrl();
    }

    protected override void OnEnable()
    {
        CharacterEvents.OnCharacterPropertiesChanged += DisplayCharacterStatsUI;
        CharacterEvents.OnDamageSourceChanged += SetDamageSource;
    }
    protected override void OnDisable()
    {
        CharacterEvents.OnCharacterPropertiesChanged -= DisplayCharacterStatsUI;
        CharacterEvents.OnDamageSourceChanged -= SetDamageSource;
    }

    protected override void Start()
    {
        base.Start();
        StartCoroutine(LoadCharacterProperties());
    }
    protected virtual void LoadCharacterStats()
    {
        if (this.characterStats != null) return;
        this.characterStats = FindAnyObjectByType<CharacterStats>();
    }
    protected virtual void LoadCharacterCtrl()
    {
        if (this.characterCrtl != null) return;
        this.characterCrtl = FindAnyObjectByType<CharacterCtrl>();
    }

    protected IEnumerator LoadCharacterProperties()
    {
        yield return new WaitUntil(() => this.characterCrtl != null && this.characterStats != null);
        //this.DisplayCharacterStatsUI();
        CharacterEvents.OnCharacterPropertiesChanged?.Invoke();
    }

    protected virtual void Update()
    {
        //this.DisplayCharacterStatsUI();
    }
    protected virtual void DisplayCharacterStatsUI()
    {
        float atk = 0;
        float defense = 0;
        float critDamage = 0;
        float critRate = 0;
        float bonusDamage = 0;
        if (this.characterCrtl != null) bonusDamage = this.characterCrtl.GetDamageFromStats();
        if (this.characterStats != null)
        {
            atk = this.damageSender != null ? this.damageSender.GetFinalDamage() : this.characterStats.currentAtk + bonusDamage;
            defense = this.characterStats.currentDef;
            critDamage = (int)this.characterStats.currentCritDamage;
            critRate = (int)this.characterStats.currentCritRate;
        }

        UIManager.Instance.UpdateCharacterStatsUI(atk, defense, critRate, critDamage/*, bonusDamage*/);
    }

    public virtual void SetDamageSource(DamageSender damageSender)
    {
        this.damageSender = damageSender;
        CharacterEvents.OnCharacterPropertiesChanged?.Invoke();
    }
}
