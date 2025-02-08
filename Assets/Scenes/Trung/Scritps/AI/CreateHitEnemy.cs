using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateHitEnemy : MonoBehaviour
{
    [SerializeField] private bool CanTakeDamage = false;
    private void OnEnable()
    {
        CanTakeDamage = false;
    }
    private void OnDisable()
    {
        CanTakeDamage = false;
    }
    [SerializeField] private EnemySO enemySO;
    private void OnTriggerEnter(Collider other)
    {
        ThirdPersonController player = other.GetComponent<ThirdPersonController>();
        if (player != null && !CanTakeDamage)
        {
            Debug.Log("Đã va chạm với Player");
            CanTakeDamage=true;
        }
    }
}
