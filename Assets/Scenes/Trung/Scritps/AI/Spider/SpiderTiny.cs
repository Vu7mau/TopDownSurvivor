using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class SpiderTiny : Zombie_FireFighterCtrl
{
    //[SerializeField] protected EnemyHealth spiderHealth;
    //[SerializeField] protected bool _spiderIsDead;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        //this.LoadEnemyHealth();
    }
    //protected virtual void LoadEnemyHealth()
    //{
    //    if(this.spiderHealth != null) return;
    //    this.spiderHealth = GetComponent<EnemyHealth>();
    //}
    protected override void OnEnable()
    {
        base.OnEnable();
        //this.ExplosionWhenSpiderIsDead();
    }
    //protected virtual void ExplosionWhenSpiderIsDead()
    //{
    //    StartCoroutine(ExplosionSpiderRoutine());
    //}
    private void FixedUpdate()
    {
        //this.CheckIsDead();
    }
    //private void CheckIsDead()
    //{
    //    if (this.spiderHealth.Health > 0 && !this._spiderIsDead) return;
    //    this._spiderIsDead = true;
    //}
    //private IEnumerator ExplosionSpiderRoutine()
    //{
    //    yield return new WaitUntil(() =>  this._spiderIsDead);
    //    this.Explode();
    //}
}
