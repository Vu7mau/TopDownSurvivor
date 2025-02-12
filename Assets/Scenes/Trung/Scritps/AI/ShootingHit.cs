using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingHit : MonoBehaviour
{
    private KnockBack knockBack;
    private void Awake()
    {
        knockBack = GetComponent<KnockBack>();
    }
    int dem;
    private void OnTriggerEnter(Collider collision)
    {
        StartCoroutine(knockBack.MaterialRoutine());
        Debug.Log(collision.gameObject.name);
        dem++;
        Debug.Log(dem);

    }
}
