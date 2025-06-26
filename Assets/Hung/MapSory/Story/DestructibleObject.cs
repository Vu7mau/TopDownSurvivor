using UnityEngine;

public class DestructibleObject : MonoBehaviour
{
    public int hpDoor = 10; 
    private int currentHits = 0;

    public GameObject explosionEffect;
    public float explosiveTime = 2f;
    public Transform explosivePostion;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("bullet"))
        {
            currentHits++;

            Debug.Log("Bullet hit: " + currentHits + " / " + hpDoor);

            // Tuỳ chọn: Huỷ viên đạn sau va chạm
            //Destroy(collision.gameObject);

            if (currentHits >= hpDoor)
            {
                DestroyObject();
            }
        }
    }

    private void DestroyObject()
    {
        if (explosionEffect != null && explosivePostion != null)
        {
            GameObject explosive = Instantiate(explosionEffect, explosivePostion.position, Quaternion.identity);
            Destroy(explosive,explosiveTime);
        }

        // âm thanh nổ

        Debug.Log(gameObject.name + " đã bị phá huỷ!");
        Destroy(gameObject);
    }
}
