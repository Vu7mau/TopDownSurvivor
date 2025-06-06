using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonUsurper : VuMonoBehaviour
{
    [SerializeField] protected FlameThrower flameProjectilePrefab;
    [SerializeField] protected Transform appearPosition;

    protected override void LoadComponents()
    {
        base.LoadComponents();
    }
    public virtual void ShootAttack()
    {
        flameProjectilePrefab.transform.position = appearPosition.position;
        flameProjectilePrefab.gameObject.SetActive(true);

    }
}
