using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBase : VuMonoBehaviour
{
    protected abstract void Idle();
    protected abstract void Chase();
    protected abstract void LookAtTarGet();
    protected abstract void Attack();
    protected abstract void AttackWhenNearPlayer();
    protected abstract void AttackWhenFarPlayer();
    protected abstract void Death();


}
