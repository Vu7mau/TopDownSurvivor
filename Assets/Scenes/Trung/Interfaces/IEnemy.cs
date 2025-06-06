using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IEnemy
{
    public bool HasHurtState();
    public bool HasDeadState();
}
interface INpc
{
    public void Patrol();
    public void Chase();
    public void Attack();
}
