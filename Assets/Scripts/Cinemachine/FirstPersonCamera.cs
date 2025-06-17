using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonCamera : MonoBehaviour
{
    public Transform cameraTransform; // Tham chiếu tới Camera
    public float mouseSensitivity = 100f;
    public float clampAngle = 80f;

    private float rotX = 0f; // Pitch - trục X (ngước lên xuống)
    private float rotY = 0f; // Yaw - trục Y (xoay ngang)

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // Ẩn con trỏ chuột và khóa vào màn hình
        Cursor.visible = false;

        Vector3 rot = transform.localRotation.eulerAngles;
        rotY = rot.y;
        rotX = rot.x;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        rotY += mouseX;
        rotX -= mouseY;
        rotX = Mathf.Clamp(rotX, -clampAngle, clampAngle);

        // Xoay nhân vật theo trục Y (ngang)
        transform.rotation = Quaternion.Euler(0, rotY, 0);

        // Xoay camera theo trục X (dọc)
        cameraTransform.localRotation = Quaternion.Euler(rotX, 0, 0);
    }
}
