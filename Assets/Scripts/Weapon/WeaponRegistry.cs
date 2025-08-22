using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WeaponRegistry : MonoBehaviour
{
    public static WeaponRegistry Instance { get; private set; }

    [Header("Kéo toàn bộ prefab RayCastWeapon vào đây")]
    public List<RayCastWeapon> weaponPrefabs = new();

    private Dictionary<string, RayCastWeapon> _map;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;

        _map = weaponPrefabs.Where(p => p != null)
                            .GroupBy(p => p.WeaponName)
                            .ToDictionary(g => g.Key, g => g.First());
    }

    public RayCastWeapon GetPrefab(string weaponName)
    {
        if (string.IsNullOrEmpty(weaponName)) return null;
        return _map != null && _map.TryGetValue(weaponName, out var pf) ? pf : null;
    }

    public RayCastWeapon GetPrefabByIndex(int idx)
    {
        if (weaponPrefabs == null || idx < 0 || idx >= weaponPrefabs.Count) return null;
        return weaponPrefabs[idx];
    }
}
