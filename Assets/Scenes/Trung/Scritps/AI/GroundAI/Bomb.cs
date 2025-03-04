using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : DamageSender
{
    [SerializeField] private float timeToDelete;
    private bool _canTakeDamage= false;
    [SerializeField] private int _damage;
    protected override void OnDisable()
    {
        base.OnDisable();
        _canTakeDamage = false;
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        _canTakeDamage = false;
        StartCoroutine(DeleteExplosion());
    }
    protected override void Start()
    {
        base.Start();
        _basedDamage = _damage;
    }
    private void OnTriggerEnter(Collider other)
    {
        CharacterDamageReceiver player = other.GetComponent<CharacterDamageReceiver>();
        if (player != null && !_canTakeDamage)
        {
            Rigidbody rb = player.gameObject.GetComponent<Rigidbody>();
            Send(other.transform);
            _canTakeDamage = true;
            AddForceToTarget(rb);
            Debug.Log("Đã chạm Player!");
        }
    }
    private void AddForceToTarget(Rigidbody rb)
    {
        rb.AddForce(new Vector3(0,10,0),ForceMode.Impulse);
    }
    private IEnumerator DeleteExplosion()
    {
        yield return new WaitForSeconds(timeToDelete);
        gameObject.SetActive(false);
    }
}
