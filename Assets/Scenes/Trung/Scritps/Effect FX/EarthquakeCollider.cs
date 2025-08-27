using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthquakeCollider : MonoBehaviour
{
    public BoxCollider boxCollider;
    public float expandSpeed = 5f;   // tốc độ dài ra
    public float maxLength = 30f;

    [SerializeField] protected Vector3 offsetSize; // nếu muốn dịch thêm
    [SerializeField] protected Vector3 offsetCenter; // nếu muốn dịch thêm

    private void Reset()
    {
        boxCollider = GetComponentInChildren<BoxCollider>();
        boxCollider.isTrigger = true;
    }

    private void OnEnable()
    {
        boxCollider.size = offsetSize;
        boxCollider.center = offsetCenter; // reset lại
    }

    private void Update()
    {
        if (boxCollider.size.z < maxLength)
        {
            var size = boxCollider.size;
            size.z += expandSpeed * Time.deltaTime;
            boxCollider.size = size;

            // Dời tâm collider để nó chỉ kéo về 1 phía (Z+)
            boxCollider.center = new Vector3(offsetCenter.x, offsetCenter.y, size.z / 2f + offsetCenter.z);
        }
    }
}
