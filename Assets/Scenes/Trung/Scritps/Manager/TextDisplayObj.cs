using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TextDisplayObj : PoolObj
{
    [SerializeField] private float timeToFade;
    [SerializeField] protected Color startColor;
    //[SerializeField] protected TextDisplayDespawn textDisplayDespawn;
    private float timeElapsed = 0f;
    private Vector3 moveSpeed = new Vector3(0, 100, 0);


    RectTransform textHealth;
    TextMeshProUGUI textDamage;
    public Color StartColor => startColor;

    [Header("Name the Text Comnponent!")]

    [SerializeField] protected string textName;
    public string TextName => textName;

    public override string GetName() => textName;
    protected override void Awake()
    {
        base.Awake();
        textHealth = GetComponent<RectTransform>();
        textDamage= GetComponent<TextMeshProUGUI>();
        textDamage.color = startColor;
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        this.ResetTextDisplay();
    }
    protected override void LoadComponents()
    {
        base.LoadComponents();
        //this.LoadTextDisplayDespawn();
    }
    //protected virtual void LoadTextDisplayDespawn()
    //{
    //    if (this.textDisplayDespawn != null) return;
    //    this.textDisplayDespawn = GetComponentInChildren<TextDisplayDespawn>();
    //}
    protected virtual void ResetTextDisplay()
    {
        this.timeElapsed = 0f;  
    }
    private void Update()
    {
        textHealth.position += moveSpeed * Time.deltaTime;
        timeElapsed+=Time.deltaTime;
        if (timeElapsed < timeToFade)
        {
            float fadeAlpha=startColor.a*(1- (timeElapsed/timeToFade));
            textDamage.color = new Color(startColor.r, startColor.g, startColor.b, fadeAlpha);
        }
    }
}
