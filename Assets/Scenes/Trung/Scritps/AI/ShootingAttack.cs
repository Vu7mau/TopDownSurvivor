using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingAttack : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform appearPosition;
    GameObject projectileObj;
    private void Start()
    {
        projectilePrefab.SetActive(false);
    }
    public void ShootAttack()
    {
        projectilePrefab.gameObject.transform.position = appearPosition.position;
        projectilePrefab.SetActive(true);
    }
}
