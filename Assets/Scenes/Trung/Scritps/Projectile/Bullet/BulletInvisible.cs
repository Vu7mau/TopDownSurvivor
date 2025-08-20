using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletInvisible : Projectitle
{
    [SerializeField] protected Transform trailRender;
    [SerializeField] protected float timeDelay;

    [SerializeField] protected bool isStarted = true;
    protected override void OnEnable()
    {
        base.OnEnable();
        StartCoroutine(this.TrailBulletRoutine());
    }
    public void TrailBullet()
    {
        if(this.trailRender != null) this.trailRender.gameObject.SetActive(true);
    }

    private IEnumerator TrailBulletRoutine()
    {
        if(!isStarted) yield break;
        if (this.trailRender == null) yield break;
        yield return new WaitForSeconds(timeDelay);
        this.trailRender.gameObject.SetActive(false);
    }
}
