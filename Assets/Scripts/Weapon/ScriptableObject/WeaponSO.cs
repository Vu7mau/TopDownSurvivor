using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="weaponInfo",menuName ="SO/Weapon")]
public class WeponSO : ScriptableObject
{
    [Header("Thông số của súng")]
    [SerializeField] public string weaponName;
    [SerializeField] public float _shootDelay = .2f;
    [SerializeField] public int _MaxBulletCount = 30;
    [SerializeField] public float _reloadAmmoTime = 2f;
    [SerializeField] public float zoomSpeed;
    [SerializeField] public float recoilSize;
    [SerializeField] public float recoilDuration;


    [Space]
    [Header("Cài đặt của súng")]
    [SerializeField] public LayerMask _enemyLayer;
    [SerializeField] public Texture _gunTexture;
    [SerializeField] public WeaponSlot weaponSlot;
}
