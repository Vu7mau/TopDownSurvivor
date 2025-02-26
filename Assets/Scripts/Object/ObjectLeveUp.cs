using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectLeveUp : VuMonoBehaviour
{
    [Space]
    [Header("ObjectLeveUp")]
    [SerializeField] protected int _level = 1;
    [SerializeField] protected float _expToNextLevel = 10;
    [SerializeField] protected float _currentExp = 0;



    public virtual void AddExp(float amount)
    {
        this._currentExp += amount;

        while(this.CanLevelUp())
        {
            this.LevelUp();
        }
    }

    protected virtual bool CanLevelUp()
    {
        return this._currentExp >this._expToNextLevel;
    }
    protected virtual void LevelUp()
    {
        this._currentExp -= this._expToNextLevel;
        this._level++;
        this._expToNextLevel *= 2f;
        this.ProcessLevelUp();
    }

    protected virtual void ProcessLevelUp()
    {
        
    }
}
