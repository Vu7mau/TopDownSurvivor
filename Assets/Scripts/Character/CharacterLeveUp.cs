using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Rendering.HighDefinition;
using UnityEngine;
using UnityEngine.UI;

public class CharacterLeveUp : ObjectLeveUp
{
    [Space]
    [Header("CharacterLeveUp")]
    [SerializeField] protected Slider _expSlider;
    [SerializeField] protected TMP_Text levelText;




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

        expCoroutine = null;
    }
    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadExpSlider();
    }
    protected override void Start()
    {
        base.Start();
        this.SetLevelUI(this._level);


    }
    protected virtual void LoadExpSlider()
    {
        if (this._expSlider != null) return;

        this._expSlider = GameObject.Find("ExpSlider").GetComponent<Slider>();
        Debug.Log("LoadExpSlider success " + this._expSlider.transform.name);
        this.levelText = this._expSlider.GetComponentInChildren<TMP_Text>();
        Debug.Log("LoadExpSlider success " + this.levelText.transform.name);


    }
    protected virtual void SetLevelUI(int level)
    {
        this.levelText.text = "Level " + this._level.ToString();
    }

    protected override void ProcessLevelUp()
    {
        DamagerScreen.Instance.SetLeveUpScreen();
        SoundFXManager.Instance.PlaySoundFXClip(SoundFXManager.Instance.leveUp, this.transform);
        CharacterStats.Instance.levelUpUI.ShowSkillChoices();
        this._expSlider.value = 0;
        this.SetLevelUI(this._level);
        Time.timeScale = 0;
        
    }
}
