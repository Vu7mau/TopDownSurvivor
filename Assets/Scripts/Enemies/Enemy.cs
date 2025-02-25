using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] protected DamageSender damage;

    private void Start()
    {
        damage=GetComponent<DamageSender>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (damage)
            {
                damage.Send(collision.transform);
                Debug.Log("do");
            }
        }
    }
}
