using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float lifeTime = 5f;
    private void Start()
    {
        StartCoroutine(ProjectileLifeTime(lifeTime));
    }
    private void OnCollisionEnter(Collision other)
    {
        gameObject.SetActive(false);
    }
    private IEnumerator ProjectileLifeTime(float time)
    {
        yield return new WaitForSeconds(time);
        gameObject.SetActive(false);
    }
}
