using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemDrops", menuName = "Items/ItemDrops")]
public class ItemDropSO : ScriptableObject
{
    [SerializeField] private List<ItemDrop> itemDrops;
    public List<ItemDrop> ItemDrops { get => this.itemDrops; }


}
[System.Serializable]
public class ItemDrop
{
    [Header("Vật phẩm rơi")]
    public PowerUpItem itemPrefab;    // Prefab vật phẩm rơi

    [Space]
    [Header("Tỉ lệ rơi")]
    [Range(0f, 100f)]
    public float dropChance;         // Tỉ lệ rơi (%)

    [Space]
    [Header("Số lượng tối đa")]
    public int maxAmount;


}
