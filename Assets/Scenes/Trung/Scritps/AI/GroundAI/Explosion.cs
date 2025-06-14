using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : PoolObj
{
    [SerializeField] private float timeToDelete;
    protected bool _canTakeDamage= false;
    public override string GetName() => "Explosion";
    protected override void OnDisable()
    {
        base.OnDisable();
        this.LoadStateDefault();
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        this.LoadStateDefault();
        StartCoroutine(this.DeleteExplosion());
    }
    protected virtual void LoadStateDefault()
    {
        this._canTakeDamage = false;
    }
    protected override void Start()
    {
        base.Start();
    }
    private void OnTriggerEnter(Collider other)
    {
        CharacterDamageReceiver player = other.GetComponent<CharacterDamageReceiver>();
        if (player != null && !_canTakeDamage)
        {
            Rigidbody rb = player.gameObject.GetComponent<Rigidbody>();
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
