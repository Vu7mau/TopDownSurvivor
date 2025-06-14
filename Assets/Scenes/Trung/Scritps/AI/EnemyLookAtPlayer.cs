using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLookAtPlayer : VuMonoBehaviour
{
    [SerializeField] protected Transform playerPosition;

    protected override void LoadComponents()
    {
        base.LoadComponents();
    }
    protected virtual void LoadPlayerPosition()
    {
        if (this.playerPosition != null) return;
        this.playerPosition = FindAnyObjectByType<CharacterCtrl>().transform;
    }
    void Update()
    {
        transform.LookAt(this.playerPosition);
    }
}
