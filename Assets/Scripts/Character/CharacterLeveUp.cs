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
    [SerializeField] protected Image _expFillImage; // Thay vì Slider
    [SerializeField] protected TMP_Text levelText;
    [SerializeField] protected TMP_Text progressText;



    protected Coroutine expCoroutine;
    protected float expToAdd = 0f;
    protected override void ResetValue()
    {
        base.ResetValue();
    }

    public override void AddExp(float amount)
    {

        base.AddExp(amount);
        this.SetProgressUI(this._currentExp, this._expToNextLevel);

    }



    protected virtual IEnumerator UpdateExpBar(float currentExp, float maxCurrentLevelExp)
    {
        float duration = 0.75f;
        float elapsed = 0f;

        float startValue = this._expFillImage.fillAmount;
        float targetValue = currentExp / maxCurrentLevelExp;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float value = Mathf.Lerp(startValue, targetValue, elapsed / duration);
            this._expFillImage.fillAmount = value;
            yield return null;
        }
        expCoroutine = null;
    }
    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadExpImage();
    }
    protected override void Start()
    {
        base.Start();
        this.SetLevelUI(this._level);
        this.SetProgressUI(this._currentExp, this._expToNextLevel);
    }

    protected virtual void Update()
    {


    }
    protected virtual void LoadExpImage()
    {
        if (this._expFillImage != null) return;

        Transform obj = GameObject.Find("EXPBar_Fill").transform; // tên object bạn đặt trong Hierarchy
        this._expFillImage = obj.GetComponent<Image>();
        Debug.Log("LoadExpImage success " + this._expFillImage.transform.name);
    }
    protected virtual void SetLevelUI(int level)
    {
        if (this.levelText != null)
        {
            this.levelText.text = "Level: " + this._level.ToString();
        }
    }
    protected virtual void SetProgressUI(float currentExp, float maxCurrentLevelExp)
    {
        if (this.progressText != null)
        {
            this.progressText.text = currentExp.ToString() + " / " + maxCurrentLevelExp.ToString();
        }
        this.SetEXPBarUI(this._currentExp, this._expToNextLevel);
    }

    protected virtual void SetEXPBarUI(float currentExp, float maxCurrentLevelExp)
    {
        if (this._expFillImage != null)
        {
            if (expCoroutine == null)
                StartCoroutine(this.UpdateExpBar(currentExp, maxCurrentLevelExp));
        }
    }
    protected override void ProcessLevelUp()
    {
        this.SetLevelUI(this._level);
        this.SetProgressUI(this._currentExp, this._expToNextLevel);
        //DamagerScreen.Instance.SetLeveUpScreen();
        //SoundFXManager.Instance.PlaySoundFXClip(SoundFXManager.Instance.leveUp, this.transform);
        CharacterStats characterStats = this.transform.parent.GetComponentInChildren<CharacterStats>();
        if (characterStats != null)
        {
            CharacterStats.Instance.levelUpUI.ShowSkillChoices();
            Time.timeScale = 0;
        }
        CharacterStats.Instance.levelUpUI.ShowSkillChoices();
        Time.timeScale = 0;

    }
}
