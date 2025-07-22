using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossReborn : MonoBehaviour
{
    [SerializeField] protected BossRise bossRise;
    [SerializeField] protected bool isAppear = true;
    protected virtual void OnEnable()
    {
        this.bossRise.transform.gameObject.SetActive(this.isAppear);
    }
}
