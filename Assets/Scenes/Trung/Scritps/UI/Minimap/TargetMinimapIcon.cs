using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetMinimapIcon : MonoBehaviour
{
    public Transform player;           // Player transform
    public Transform target;           // Enemy or object to track
    public RectTransform minimapIcon;  // Icon UI
    public float minimapRadius = 100f; // Radius of minimap in UI


    private void OnEnable()
    {
        player = FindAnyObjectByType<CharacterAnimHandle>().transform;
        target = GetComponentInParent<EnemyCtrl>().transform;
    }
    void Update()
    {
        Vector3 offset = target.position - player.position;
        Vector2 iconPos = new Vector2(offset.x, offset.z); // assuming Y is up

        // Nếu vượt quá bán kính, clamp lại
        if (iconPos.magnitude > minimapRadius)
        {
            iconPos = iconPos.normalized * minimapRadius;
        }

        minimapIcon.anchoredPosition = iconPos;
    }
}
