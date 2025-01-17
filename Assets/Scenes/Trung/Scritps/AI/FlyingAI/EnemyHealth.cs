using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyHealth : MonoBehaviour,IEnemy
{
    [SerializeField] private EnemySO enemySO;
    private float _health;
    private Animator _animator;
    private bool _canTakeDamage = true;
    public float Health
    {
        get { return _health; }
        set { _health = value; }
    }
    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }
    private void Start()
    {
        Health = enemySO.Health;
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
            Debug.Log("Máu quái còn: " + Health);
            _animator.SetTrigger("damage");
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        CharacterCtrl characterCtrl = collision.gameObject.GetComponent<CharacterCtrl>();
        if(characterCtrl != null)
        {
            TakeDamage(50f);
        }
    }
    public void Die()
    {
        _canTakeDamage = false;
        Debug.Log("Quái đã chết");
        _animator.SetTrigger("die");


        //Sau khi quái chết thì 2 giây sau quái sẽ biến mất
        StartCoroutine(DeleteEnemyRoutine());
    }
    private IEnumerator DeleteEnemyRoutine()
    {
        yield return new WaitForSeconds(2f);
        gameObject.SetActive(false);
    }
}
