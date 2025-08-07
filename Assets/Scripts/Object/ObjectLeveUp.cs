using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectLeveUp : VuMonoBehaviour
{
    [Space]
    [Header("ObjectLeveUp")]
    [SerializeField] protected int _level = 1;
    [SerializeField] protected int _maxLevel = 99;
    [SerializeField] protected float _expToNextLevel = 10;
    [SerializeField] protected float _currentExp = 0;



    public virtual void AddExp(float amount)
    {
        if(!this.ReachMaxLevel()) this._currentExp += amount;

        while(this.CanLevelUp())
        {
            this.LevelUp();
        }
    }

    protected virtual bool CanLevelUp()
    {
        return this._level < this._maxLevel && this._currentExp >= this._expToNextLevel;
    }
    protected virtual bool ReachMaxLevel()
    {
        return this._level == this._maxLevel && this._currentExp >= this._expToNextLevel;
    }
    protected virtual void LevelUp()
    {
        if (this._level >= this._maxLevel) return;

        this._currentExp -= this._expToNextLevel;
        this._level++;

        // Nếu sau khi tăng level mà đạt maxLevel, thì set exp đúng giới hạn
        if (this.ReachMaxLevel())
        {
            this._currentExp = this._expToNextLevel; // Giữ thanh đầy 100%
        }
        else
        {
            this._expToNextLevel *= 2f;
        }

        this.ProcessLevelUp();
    }
    protected virtual void ProcessLevelUp()
    {
        
    }
}
