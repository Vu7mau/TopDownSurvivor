using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaticleFX : VuMonoBehaviour
{
    [SerializeField] protected float time;

    protected override void OnEnable()
    {
        base.OnEnable();
        this.Despawn(this.time);
    }

    protected virtual void Despawn(float time)
    {
        StartCoroutine(DespawnRoutine(time));
    }

    private IEnumerator DespawnRoutine(float time)
    {
        yield return new WaitForSeconds(time);
        this.gameObject.SetActive(false);
    }
}
