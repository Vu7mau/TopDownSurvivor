using UnityEngine;

public class ArrowFloat : MonoBehaviour
{
    public enum FloatDirection { UpDown, LeftRight, ForwardBackward, Custom }

    [Header("Float Settings")]
    [Tooltip("Hướng dao động.")]
    public FloatDirection floatDirection = FloatDirection.UpDown;

    [Tooltip("Vector hướng tùy chỉnh (chỉ dùng nếu chọn Custom).")]
    public Vector3 customDirection = Vector3.up;

    [Tooltip("Biên độ dao động (khoảng cách tối đa).")]
    public float floatAmplitude = 0.25f;

    [Tooltip("Tốc độ dao động (chu kỳ/s).")]
    public float floatFrequency = 1.5f;

    [Header("Rotation Settings")]
    [Tooltip("Có xoay quanh Y không? (tạo hiệu ứng quay).")]
    public bool rotate = true;
    public float rotateSpeed = 90f; // độ/giây

    private Vector3 _startPos;
    private Vector3 _direction;

    void Start()
    {
        _startPos = transform.localPosition;

        // Gán hướng dao động theo lựa chọn
        switch (floatDirection)
        {
            case FloatDirection.UpDown: _direction = Vector3.up; break;
            case FloatDirection.LeftRight: _direction = Vector3.right; break;
            case FloatDirection.ForwardBackward: _direction = Vector3.forward; break;
            case FloatDirection.Custom: _direction = customDirection.normalized; break;
        }
    }

    void Update()
    {
        // Dao động theo hướng đã chọn
        float offset = Mathf.Sin(Time.time * floatFrequency) * floatAmplitude;
        transform.localPosition = _startPos + _direction * offset;

        // Optional rotate
        if (rotate)
        {
            transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime, Space.World);
        }
    }
}
