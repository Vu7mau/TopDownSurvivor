using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering.HighDefinition;
using UnityEngine;
using UnityEngine.UI;

public class CharacterLeveUp : ObjectLeveUp
{
    [Space]
    [Header("CharacterLeveUp")]
    [SerializeField] protected Slider _expSlider;




    protected Coroutine expCoroutine;
    protected float expToAdd = 0f;
    protected override void ResetValue()
    {
        base.ResetValue();
    }

    public override void AddExp(float amount)
    {
     
        base.AddExp(amount);
        if (expCoroutine == null)
            StartCoroutine(this.UpdateExpBar(amount));
   
    }



    protected virtual IEnumerator UpdateExpBar(float amount)
    {
        this._expSlider.maxValue = this._expToNextLevel;
        float duration = .75f;
        float elapsed = 0;
        float startValue = this._expSlider.value;
        float targetValue = _currentExp;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float value = Mathf.Lerp(startValue, targetValue, elapsed / duration);
            this._expSlider.value = value;
            yield return null;
        }
        
        expCoroutine=null;
    }
    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadExpSlider();
    }
    protected virtual void LoadExpSlider()
    {
        if (this._expSlider != null) return;

        this._expSlider = GameObject.Find("ExpSlider").GetComponent<Slider>();
        Debug.Log("LoadExpSlider success " + this._expSlider.transform.name);
    }
    protected override void ProcessLevelUp()
    {
        CharacterStats.Instance.levelUpUI.ShowSkillChoices();

        this._expSlider.value = 0;

        Time.timeScale = 0;
    }
}
