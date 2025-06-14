using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class TextDisplayManager : VuMonoBehaviour
{
    [SerializeField] protected TextDisplayObj textDisplayPrefab;
    [SerializeField] protected TextDisplaySpawner _spawner;
    //[SerializeField] private GameObject coinTextPrefab;
    //[SerializeField] private GameObject ExpTextPrefab;

    //[SerializeField] protected Canvas _myCanvas;
    [SerializeField] protected Canvas textCanvas;

    protected override void OnEnable()
    {
        base.OnEnable();
        CharacterEvents.characterDamaged += CharacterTookDamage;
        //CharacterEvents.characterTookItem += CharacterTookItem;
        //CharacterEvents.characterTookExp += CharacterTookExp;
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        CharacterEvents.characterDamaged -= CharacterTookDamage;
        //CharacterEvents.characterTookItem -= CharacterTookItem;
        //CharacterEvents.characterTookExp -= CharacterTookExp;
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadTextPrefab();
        this.LoadCanvas();
        this.LoadTextDisplaySpawner();
    }

    protected virtual void LoadTextPrefab()
    {
        if (this.textDisplayPrefab != null) return;
        List<TextDisplayObj> allMyComponents = ComponentFinder.FindAllComponentsInScene<TextDisplayObj>();
        foreach(TextDisplayObj myComponent in allMyComponents)
        {
            if(myComponent.TextName == "DamageText")
            {
                this.textDisplayPrefab = myComponent;
                break;
            }
        }
    }
    protected virtual void LoadTextDisplaySpawner()
    {
        if (this._spawner != null) return;
        this._spawner = FindAnyObjectByType<TextDisplaySpawner>();
        Debug.Log(transform.name + ":Load Text Prefab!");
    }
    protected virtual void LoadCanvas()
    {
        if (this.textCanvas != null) return;
        this.textCanvas = GetComponentInChildren<Canvas>();
        Debug.Log(transform.name + ":Load Text Canvas!");
    }

    public virtual void CharacterTookDamage(GameObject character, float damageReceived)
    {
        Vector3 spamPosition = Camera.main.WorldToScreenPoint(character.transform.position);
        TextDisplayObj newTextDisplay = this._spawner.Spawn(this.textDisplayPrefab, spamPosition);
        if(newTextDisplay ==  null) return;
        newTextDisplay.transform.SetParent(this.textCanvas.transform);
        TMP_Text tmp_text = newTextDisplay.GetComponent<TMP_Text>();
        tmp_text.text = "-" + damageReceived.ToString();
        tmp_text.color = this.textDisplayPrefab.StartColor;
        this.LoadTextFormat(tmp_text, this.textDisplayPrefab);
        Debug.Log("Text damage đã hiện!");
    }
    protected virtual void LoadTextFormat(TMP_Text text,TextDisplayObj textDisplayObj)
    {
        if(text == null) return;
        text.font = textDisplayObj.GetComponent<TMP_Text>().font;
        text.fontStyle = textDisplayObj.GetComponent<TMP_Text>().fontStyle;
        text.fontSize = textDisplayObj.GetComponent<TMP_Text>().fontSize;
        text.alignment = textDisplayObj.GetComponent<TMP_Text>().alignment;
        text.enableWordWrapping = textDisplayObj.GetComponent<TMP_Text>().enableWordWrapping;
        text.overflowMode = textDisplayObj.GetComponent<TMP_Text>().overflowMode;
        text.horizontalMapping = textDisplayObj.GetComponent<TMP_Text>().horizontalMapping;
        text.verticalMapping = textDisplayObj.GetComponent<TMP_Text>().verticalMapping;
    }
}
