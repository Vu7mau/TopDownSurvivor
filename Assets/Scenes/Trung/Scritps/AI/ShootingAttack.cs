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
        projectileObj = Instantiate(projectilePrefab);
        projectileObj.transform.parent = transform;
        projectileObj.gameObject.SetActive(false);
    }
    public void ShootAttack()
    {
        projectileObj.gameObject.transform.position = appearPosition.position;
        projectileObj.SetActive(true);
    }
}
