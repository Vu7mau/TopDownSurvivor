using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderHPSort : VuMonoBehaviour
{

    [SerializeField] protected RectTransform bossPanelCanvas;
    [SerializeField] protected Transform defaultParent;
    [SerializeField] private HpBarObj hpBarObj;

    [SerializeField] protected Transform canvasParent;
    [SerializeField] protected List<Transform> canvasChild;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadHpBarObj();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        this.LoadBossPanel();
    }

    protected virtual void LoadBossPanel()
    {
        this.defaultParent = this.transform.parent;
        var obj1 = GameObject.Find("BossPanelCanvas");
        if (obj1 != null) this.bossPanelCanvas = obj1.GetComponent<RectTransform>();
        this.SetPositionHealthBar();
    }
    protected virtual void LoadHpBarObj()
    {
        if (this.hpBarObj != null) return;
        this.hpBarObj = GetComponentInParent<HpBarObj>();
    }

    protected virtual void SetDefault()
    {
        if(this.canvasParent != null)
        {
            this.canvasParent.transform.gameObject.SetActive(true);
        }
            
            this.canvasParent.transform.parent = this.transform;
        if(this.canvasChild.Count > 0)
        {
            foreach (Transform child in this.canvasChild)
            {
                child.transform.parent = this.canvasParent;
            }
        }
    }

    protected virtual void SetNew()
    {
        if (this.canvasChild.Count > 0)
        {
            foreach (Transform child in this.canvasChild)
            {
                child.transform.parent = null;
                child.transform.parent = this.transform;
            }
        }
        if (this.canvasParent != null)
        {
            this.canvasParent.transform.gameObject.SetActive(false);
        }
    }

    protected virtual void SetPositionHealthBar()
    {
        this.hpBarObj.transform.parent = this.defaultParent;
        this.SetDefault();
        if (this.hpBarObj != null && this.bossPanelCanvas != null)
        {
            this.hpBarObj.transform.parent = this.bossPanelCanvas;
        }
        this.SetNew();
    }
}
