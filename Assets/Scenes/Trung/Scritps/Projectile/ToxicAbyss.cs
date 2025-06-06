using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToxicAbyss : Projectitle
{
    public override string GetName() => "ToxicAbyss";
    public float speed = 10f;
    private Vector3 direction;

    protected override void LoadComponents()
    {
        base.LoadComponents();
    }

    public void ShootAt(Vector3 target)
    {
        direction = (target - transform.position).normalized;
    }

    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }
    //private void OnTriggerEnter(Collider other)
    //{
    //    CharacterDamageReceiver player = other.GetComponent<CharacterDamageReceiver>();
    //    if (player != null)
    //    {
    //        player.Deduct((int)this.damage);

    //    }
    //}
}
