using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLevelSystem : MonoBehaviour
{
    public int Level { get; private set; } = 1;
    public int CurrentEXP { get; private set; } = 0;
    public int EXPToNextLevel => Mathf.FloorToInt(100 * Mathf.Pow(1.2f, Level - 1)); // tăng EXP mỗi cấp

    public event Action<int> OnLevelUp;
    public event Action<int, int> OnExpChanged;

    [SerializeField] protected bool isCountLevel = true;

    public void AddEXP(int amount)
    {
        if (!this.isCountLevel) return;
        CurrentEXP += amount;
        OnExpChanged?.Invoke(CurrentEXP, EXPToNextLevel);

        while (CurrentEXP >= EXPToNextLevel)
        {
            CurrentEXP -= EXPToNextLevel;
            Level++;
            OnLevelUp?.Invoke(Level);
            OnExpChanged?.Invoke(CurrentEXP, EXPToNextLevel);
            //this.ProcessLevelUp();
        }
    }
    public virtual void ProcessLevelUp()
    {
        DamagerScreen.Instance.SetLeveUpScreen();
        SoundFXManager.Instance.PlaySoundFXClip(SoundFXManager.Instance.leveUp, this.transform);
        CharacterStats.Instance.levelUpUI.ShowSkillChoices();
        Time.timeScale = 0;
    }
}
