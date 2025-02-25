using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateHitEnemy : DamageSender
{
    [SerializeField] private bool CanTakeDamage = false;
    private int _damage;
     int dem;
    protected override void OnEnable()
    {
        base.OnEnable();
        CanTakeDamage = false;
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        CanTakeDamage = false;
    }
    protected override void Start()
    {
        base.Start();
        _damage = (int)enemySO.Damage;
        this.SetDamage(this._damage);
    }
    [SerializeField] private EnemySO enemySO;
    private void OnTriggerEnter(Collider other)
    {

        // Debug.Log("Va cham!");
        //Nếu va chạm với Player thì Player sẽ bị mất máu
        //CharacterCtrl player = other.GetComponent<CharacterCtrl>();
        CharacterDamageReceiver player = other.GetComponent<CharacterDamageReceiver>();
        if (player != null && !CanTakeDamage)
        {
            this.Send(other.transform);
            dem++;
            Debug.Log($"Đã va chạm với Player {dem} lần");
            CanTakeDamage = true;
        }
    }
}
