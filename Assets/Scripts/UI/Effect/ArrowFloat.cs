using UnityEngine;

public class ArrowFloat : MonoBehaviour
{
    [Header("Float Settings")]
    [Tooltip("Biên độ dao động (độ cao tối đa).")]
    public float floatAmplitude = 0.25f;
    [Tooltip("Tốc độ dao động (chu kỳ/s).")]
    public float floatFrequency = 1.5f;

    [Header("Rotation Settings")]
    [Tooltip("Có xoay quanh Y không? (tạo hiệu ứng quay).")]
    public bool rotate = true;
    public float rotateSpeed = 90f; // độ/giây

    private Vector3 _startPos;

    void Start()
    {
        _startPos = transform.localPosition;
    }

    void Update()
    {
        // Up-down
        float newY = _startPos.y + Mathf.Sin(Time.time * floatFrequency) * floatAmplitude;
        Vector3 pos = _startPos;
        pos.y = newY;
        transform.localPosition = pos;

        // Optional rotate
        if (rotate)
        {
            transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime, Space.World);
        }
    }
}
