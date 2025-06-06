using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class SpiderTiny : EnemyAI
{
    private enum TypeBomb { None, OneTouch, OnTime }
    [SerializeField] private TypeBomb type;
    [SerializeField] private DragonUsurper shootingAttack;
    private void Update()
    {

    }
}
