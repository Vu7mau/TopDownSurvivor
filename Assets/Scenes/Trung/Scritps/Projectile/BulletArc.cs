using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BulletArc : Projectile
{
    [SerializeField] private float height;
    [SerializeField] private Vector3 offSet;
    private float duration;
    private Vector3 targetPosition;
    private void OnEnable()
    {
        Move();
    }
    private void Move()
    {
        targetPosition = FindAnyObjectByType<CharacterCtrl>().transform.position;
        Vector3 startPosition = transform.position;
        Vector3 endPosition = targetPosition + offSet;
        duration = lifeTime;
        Vector3 midPoint = (startPosition + endPosition) / 2;
        midPoint.y += height;
        Vector3[] path = new Vector3[] { startPosition, midPoint, endPosition };
        transform.DOPath(path, duration, PathType.CatmullRom).SetEase(Ease.Linear).OnComplete(() => DeleteBullet());
    }
    private void OnTriggerEnter(Collider other)
    {
        CharacterDamageReceiver player = other.GetComponent<CharacterDamageReceiver>();
        if (player != null)
        {
            player.Deduct((int)this.damage);
            DeleteBullet();
        }
    }
    private void DeleteBullet()
    {
        gameObject.SetActive(false);
    }
}
