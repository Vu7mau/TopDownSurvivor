using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameThrower : VuMonoBehaviour
{
    [SerializeField] protected float lifeTime;
    [SerializeField] protected float currentTime;
    protected override void OnEnable()
    {
        base.OnEnable();
        this.LoadCurrentTime();
    }
    protected virtual void LoadCurrentTime()
    {
        this.currentTime = this.lifeTime;
    }
    protected void Update()
    {
        this.CheckDespawn();
    }
    protected virtual void CheckDespawn()
    {
        currentTime -= Time.deltaTime;
        if (this.currentTime > 0) return;
        this.transform.gameObject.SetActive(false);
    }
}
