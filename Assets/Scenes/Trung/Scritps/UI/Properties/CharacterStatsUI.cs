using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStatsUI : VuMonoBehaviour
{
    [SerializeField] protected CharacterStats characterStats;
    [SerializeField] protected CharacterCtrl characterCrtl;

    [SerializeField] protected bool isOnStats = false;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadCharacterStats();
        this.LoadCharacterCtrl();
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

    protected virtual void Update()
    {
        this.DisplayCharacterStatsUI();
    }

    protected virtual void DisplayCharacterStatsUI()
    {
        float atk = 0;
        float defense = 0;
        float critDamage = 0;
        float critRate = 0;
        float bonusDamage = 0;
        if (this.characterStats != null)
        {
            atk  = this.characterStats.currentAtk;
            defense= this.characterStats.currentDef;
            critDamage = (int)this.characterStats.currentCritDamage;
            critRate = (int)this.characterStats.currentCritRate;
        }
        if(this.characterCrtl != null) bonusDamage = (int)this.characterCrtl.GetDamageFromStats();

        UIManager.Instance.UpdateCharacterStatsUI(atk, defense, critRate, critDamage, bonusDamage);
    }
}
