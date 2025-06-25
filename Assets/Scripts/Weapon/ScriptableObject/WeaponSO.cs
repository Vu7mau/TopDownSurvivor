using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName ="weaponInfo",menuName ="SO/Weapon")]
public class WeponSO : ScriptableObject
{
    [Header("Thông số của súng")]
    [SerializeField] public string weaponName;
    [SerializeField] public float shootDelay = .2f;
    [SerializeField] public int totalAmmo = 10;
    [SerializeField] public int maxBulletCount = 30;
    [SerializeField] public float reloadAmmoTime = 2f;
    [SerializeField] public float zoomSpeed;
    [SerializeField] public float recoilSize;
    [SerializeField] public float recoilDuration;


    [Space]
    [Header("Cài đặt của súng")]
    [SerializeField] public LayerMask enemyLayer;
    [SerializeField] public Sprite gunImage;
    [SerializeField] public WeaponSlot weaponSlot;
}
