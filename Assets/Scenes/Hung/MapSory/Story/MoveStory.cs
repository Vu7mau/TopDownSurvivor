using UnityEngine;

public class SimpleCubeMovement : MonoBehaviour
{
    public float moveSpeed = 5f;

    void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal"); // A/D hoặc ←/→
        float moveZ = Input.GetAxisRaw("Vertical");   // W/S hoặc ↑/↓

        Vector3 moveDir = new Vector3(moveX, 0f, moveZ).normalized;
        transform.position += moveDir * moveSpeed * Time.deltaTime;
    }
}
