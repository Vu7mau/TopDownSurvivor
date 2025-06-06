using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public abstract class BulletArc : PoolObj
{
    
    [SerializeField] protected float curveHeight = 2f;
    [SerializeField] protected float duration = 1f;


    [SerializeField] protected float damage;
    protected Tween moveTween;

    protected override void OnEnable()
    {
        base.OnEnable();
    }
    protected override void LoadComponents()
    {
        base.LoadComponents();
    }
    
    public virtual void Shoot(Vector3 startPos, Vector3 endPos)
    {
        // Reset vị trí ban đầu
        transform.position = startPos;

        // Nếu tween cũ còn tồn tại, kill nó
        moveTween?.Kill();

        // Tạo đường bay cong với 3 điểm: start -> giữa -> end
        Vector3 middle = Vector3.Lerp(startPos, endPos, 0.5f) + Vector3.up * curveHeight;
        Vector3[] path = new Vector3[] { startPos, middle, endPos };

        // Bắt đầu tween mới
        moveTween = transform.DOPath(path, duration, PathType.CatmullRom)
                             .SetEase(Ease.Linear)
                             //.OnComplete(() =>
                             //{
                             //    // Tắt viên đạn sau khi bay xong (trả về pool)
                             //    gameObject.SetActive(false);
                             //})
                             ;
    }
    private void OnTriggerEnter(Collider other)
    {
        CharacterDamageReceiver player = other.GetComponent<CharacterDamageReceiver>();
        if (player != null)
        {
            player.Deduct((int)this.damage);
        }
    }
}
