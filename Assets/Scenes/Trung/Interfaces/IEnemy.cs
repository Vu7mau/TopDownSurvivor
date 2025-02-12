using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IEnemy
{
    public void TakeDamage(int damage);
    public void OnDead();
}
interface INpc
{
    public void Patrol();
    public void Chase();
    public void Attack();
}
