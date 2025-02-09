using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateHitEnemy : MonoBehaviour
{
    [SerializeField] private bool CanTakeDamage = false;
    int dem;
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
        //CharacterCtrl player = other.GetComponent<CharacterCtrl>();
        ThirdPersonController player = other.GetComponent<ThirdPersonController>();
        if (player != null && !CanTakeDamage)
        {
            dem++;
            Debug.Log($"Đã va chạm với Player {dem} lần");
            CanTakeDamage=true;
        }
    }
}
