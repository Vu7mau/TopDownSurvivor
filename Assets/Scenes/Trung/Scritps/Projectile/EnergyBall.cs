using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyBall : Projectitle
{
    [SerializeField] protected string Name;
    public override string GetName() => Name;
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("BulletEnemy") && !other.transform.CompareTag("Enemy") && !other.transform.CompareTag("bullet"))
        {
            this.Despawn.DoDespawn();
        }
    }
}
