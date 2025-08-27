using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderHPCtrl : VuMonoBehaviour
{
    [SerializeField] protected RectTransform rect;
    [SerializeField] protected bool isArrive;
    public bool IsArrive { get => this.isArrive; set => this.isArrive = value; }

    /*[SerializeField] */protected RectTransform defaultPos;

    private Coroutine coroutine;
    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadRectTransform();
    }

    protected virtual void LoadRectTransform()
    {
        if (this.rect != null) return;
        this.rect = GetComponent<RectTransform>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        this.SetSLiderHP();
    }

    protected override void Start()
    {
        base.Start();
        //this.defaultPos.anchoredPosition = this.rect.anchoredPosition;
    }

    public virtual void UpdateDisplayHP()
    {
        if (isArrive)
        {
            if (coroutine != null) StopCoroutine(coroutine);
            coroutine = StartCoroutine(DisplayHPRoutine());
        }
        else
        {
            DisplayHPSlider();
        }
    }

    IEnumerator DisplayHPRoutine()
    {
        DisplayHPSlider();
        yield return new WaitForSeconds(3f);
        HideHPSlider();
        this.coroutine = null;
    }

    protected virtual void DisplayHPSlider()
    {
        if (this.rect == null) return;
        this.rect.anchoredPosition = /*this.defaultPos.anchoredPosition*/ new Vector2(0, 0);
    }
    public virtual void HideHPSlider()
    {
        if (this.rect == null) return;
        this.rect.anchoredPosition = new Vector2(this.rect.anchoredPosition.x, this.rect.anchoredPosition.y + 1000f);
    }

    public virtual void SetSLiderHP()
    {
        if (this.isArrive) HideHPSlider();
        else this.DisplayHPSlider();         
    }
}
