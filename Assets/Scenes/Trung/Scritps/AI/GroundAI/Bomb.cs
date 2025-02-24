using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField] private float timeToDelete;
    private bool _canTakeDamage= false;
    private void OnDisable()
    {
        _canTakeDamage = false;
    }
    private void OnEnable()
    {
        _canTakeDamage = false;
        StartCoroutine(DeleteExplosion());
    }
    private void OnTriggerEnter(Collider other)
    {
        CharacterCtrl player = other.gameObject.GetComponent<CharacterCtrl>();
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
