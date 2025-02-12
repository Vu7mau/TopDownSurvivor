using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField] private LayerMask groundMask;
    private Rigidbody rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void Start()
    {
        rb.isKinematic = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        //Nếu va chạm với mặt đất thì nằm trên mặt đất
        if (((1 << other.gameObject.layer) & groundMask) != 0)
        {
            rb.isKinematic = true;
            Debug.Log("Đã va chạm với ground!");
        }

        //Nếu va chạm với người chơi thì xóa powerup
        ThirdPersonController player = other.GetComponent<ThirdPersonController>();
        if(player != null)
        {
            Destroy(gameObject);
            Debug.Log("Bạn đã thu thập được kim cương!");
        }
    }
}
