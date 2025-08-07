using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PowerUpItem : PoolObj
{
    [SerializeField] private LayerMask groundLayer; // Gán layer Ground từ Inspector
    private Rigidbody rb;

    protected override void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        rb.useGravity = true;
        rb.isKinematic = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        // Kiểm tra xem object va chạm có nằm trong LayerMask groundLayer hay không
        if ((groundLayer.value & (1 << other.gameObject.layer)) != 0)
        {
            rb.velocity = Vector3.zero;
            rb.useGravity = false;
            rb.isKinematic = true;

            // Snap lên đúng bề mặt (nếu cần)
            Vector3 pos = transform.position;
            pos.y = other.ClosestPoint(transform.position).y;
            transform.position = pos;
        }
    }
}
