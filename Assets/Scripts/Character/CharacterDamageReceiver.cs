using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterDamageReceiver : DamageReceiver
{
    protected override void OnDead()
    {
        Debug.Log("PLayer Death");
    }

    
}
