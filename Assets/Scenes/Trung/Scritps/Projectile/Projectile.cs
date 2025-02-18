using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] protected float lifeTime;
    [SerializeField] protected float damage;
    private void OnEnable()
    {
        Delete();
    }
    public void Delete()
    {
        StartCoroutine(ProjectileLifeTime(lifeTime));
    }
    private IEnumerator ProjectileLifeTime(float time)
    {
        yield return new WaitForSeconds(time);
        gameObject.SetActive(false);
    }
}
