using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IEnemy
{
    public void TakeDamage(float damage);
    public void Die();
}
interface INpc
{
    public void Patrol();
    public void Chase();
    public void Attack();
}
