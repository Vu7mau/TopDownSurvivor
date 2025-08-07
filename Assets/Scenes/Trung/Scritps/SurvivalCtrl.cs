using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurvivalCtrl : VuMonoBehaviour
{
    [SerializeField] protected Transform panelWave;
    [SerializeField] protected Transform panelStats;

    protected bool isPanelWave = true;

    protected override void OnEnable()
    {
        this.panelWave.gameObject.SetActive(true);
        this.panelStats.gameObject.SetActive(false);
    }

    protected virtual void Update()
    {
        this.CtrlPanelSurvival();
    }

    protected virtual void CtrlPanelSurvival()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (this.isPanelWave)
            {
                if(this.panelWave != null && this.panelStats != null)
                {
                    this.panelWave.gameObject.SetActive(false);
                    this.panelStats.gameObject.SetActive(true);
                }
            }
            else
            {
                if (this.panelWave != null && this.panelStats != null)
                {
                    this.panelWave.gameObject.SetActive(true);
                    this.panelStats.gameObject.SetActive(false);
                }
            }
            this.isPanelWave = !this.isPanelWave;
        }
    }
}
