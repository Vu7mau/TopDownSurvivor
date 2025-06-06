using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAbstract : MonoBehaviour
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
            }
            //CharacterCtrl.Instance.CharacterLeveUp.AddExp(2);
        }
    }
}
