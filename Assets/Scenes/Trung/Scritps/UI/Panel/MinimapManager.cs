using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapManager : MonoBehaviour
{
    public RectTransform minimapRect;   // RawImage minimap
    public Transform player;            // Player
    public float minimapRadius = 100f;  // Bán kính minimap trong UI (anchoredPosition)


    public List<Transform> enemies;         // Danh sách quái

    public static MinimapManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        player = FindAnyObjectByType<CharacterAnimHandle>().transform;
    }
    void Update()
    {
        //foreach (Transform enemy in enemies)
        //{
        //    // Kiểm tra có icon chưa
        //    RectTransform icon = enemy.GetComponentInChildren<TargetMinimapIcon>().GetComponent<RectTransform>();
        //    UpdateIcon(enemy, icon);
        //}
    }

    public virtual void AddIconToMinimapList(Transform icon)
    {
        if (this.enemies.Contains(icon)) return;
        this.enemies.Add(icon);
    }

    void UpdateIcon(Transform enemy, RectTransform icon)
    {
        Vector3 dir = enemy.position - player.position;
        Vector2 dir2D = new Vector2(dir.x, dir.z);

        if (dir2D.magnitude <= minimapRadius)
        {
            // Trong phạm vi minimap
            icon.anchoredPosition = dir2D;
            icon.SetParent(enemy);
        }
        else
        {
            icon.SetParent(minimapRect);
            // Ngoài minimap → ép vào viền
            Vector2 clamped = dir2D.normalized * minimapRadius;
            icon.anchoredPosition = clamped;
        }
    }
}
