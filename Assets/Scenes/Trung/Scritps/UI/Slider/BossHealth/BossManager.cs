using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class BossManager : VuMonoBehaviour
{
    [SerializeField] protected List<BossController> listBosses;

    protected void Update()
    {
        this.UpdateBosses();
    }

    protected virtual void UpdateBosses()
    {
        BossController[] list = FindObjectsByType<BossController>(FindObjectsSortMode.None);
        foreach (BossController controller in list)
        {
            this.AddBosses(controller);
        }
        if(this.listBosses.Count > 0)
        {
            if(this.listBosses.Count > 1)
            {
                foreach (BossController boss in listBosses)
                {
                    boss.GetComponentInChildren<SliderHPCtrl>().IsArrive = true;
                }
            }
            else
            {
                foreach (BossController boss in listBosses)
                {
                    boss.GetComponentInChildren<SliderHPCtrl>().IsArrive = false;
                }
            }
        }

    }

    public virtual void DisplayMySliderHP(EnemyHealth enemyHealth)
    {
        foreach(BossController boss in listBosses)
        {
            if(boss.GetComponentInChildren<EnemyHealth>() == enemyHealth)
            {
                boss.GetComponentInChildren<SliderHPCtrl>().UpdateDisplayHP();
            }
            else
            {
                boss.GetComponentInChildren<SliderHPCtrl>().HideHPSlider();
            }
        }
    }

    protected virtual void AddBosses(BossController boss)
    {
        if (this.listBosses.Contains(boss)) return;
        this.listBosses.Add(boss);
    }
}
