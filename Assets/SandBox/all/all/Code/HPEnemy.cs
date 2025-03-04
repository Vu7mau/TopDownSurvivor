using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPEnemy : MonoBehaviour
{
    public int Hp = 10;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Hp -= 10;
            if(Hp <= 0)
            {
                Die();
            }
        }
    }
    public void Die()
    {
        EnemyManagerMisson.EnemyDied(gameObject);
        Destroy(gameObject);
    }
}
