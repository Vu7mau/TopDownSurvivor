using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CanMovement : MonoBehaviour
{
    public float speed = 3f;
    private Vector3 direction;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = true;
        rb.isKinematic = false;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
    }

    public void SetRandomDirection()
    {
        float angle = Random.Range(0f, 360f);
        direction = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)).normalized;
    }

    private void FixedUpdate()
    {
        Vector3 velocity = new Vector3(direction.x, rb.velocity.y, direction.z) * speed;
        rb.velocity = velocity;

        if (!IsVisibleToCamera())
        {
            gameObject.SetActive(false);
        }
    }

    bool IsVisibleToCamera()
    {
        Vector3 viewportPos = Camera.main.WorldToViewportPoint(transform.position);
        return viewportPos.x >= 0 && viewportPos.x <= 1 &&
               viewportPos.y >= 0 && viewportPos.y <= 1 &&
               viewportPos.z > 0;
    }
}
