using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyHealth : MonoBehaviour,IEnemy
{
    [SerializeField] private EnemySO enemySO;
    private float _health;
    private Animator _animator;
    [SerializeField] private bool _canTakeDamage = true;
    private Rigidbody rb;
    private bool hasState;
    public float Health
    {
        get { return _health; }
        set { _health = value; }
    }
    private void Awake()
    {
        rb= GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
    }
    private void Start()
    {
        Health = enemySO.Health;
        rb.isKinematic = false;
    }
    public void TakeDamage(float damage)
    {
        if (!_canTakeDamage) { return; }
        Health -= damage;
        if (Health <= 0)
        {
            Die();
        }
        else
        {
            Hurt();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        //ThirdPersonController player = other.GetComponent<ThirdPersonController>();
        //if (player != null)
        //{
        //    TakeDamage(50);
        //}
    }
    public void Die()
    {
       rb.isKinematic= true;
        _canTakeDamage = false;
        Debug.Log("Quái đã chết");
        _animator.SetTrigger("die");
    }
    public void Hurt()
    {
        if (_animator != null)
        {
            hasState = _animator.HasState(0, Animator.StringToHash("getHit"));
            Debug.Log($"State tồn tại: {hasState}");
            if (hasState)
            {
                Debug.Log("Máu quái còn: " + Health);
                _animator.SetTrigger("damage");
            }
        }
    }
    public void RewardPlayerAfterEnemyDead()
    {
        Rewards.Instance.RewardGemsPlayerWhenKillEnemy(enemySO.amount_Gems, transform);
    }
    public void DeleteEnemyRoutine()
    {
        gameObject.SetActive(false);
    }
}
