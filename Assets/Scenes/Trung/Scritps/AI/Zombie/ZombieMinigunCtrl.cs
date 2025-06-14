using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ZombieMinigunCtrl : ZombieCtrl
{
    [SerializeField] protected MinigunBulletZombieSpawner minigunBulletZombieSpawner;
    [SerializeField] protected MinigunBulletZombie bulletMinigunPrefab;
    [SerializeField] protected List<Transform> listPositions;

    [SerializeField] protected int shootTime = 4;
    
    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadMinigunBulletZombie();
        this.LoadMinigunBulletZombieSpawner();
        this.LoadEndPoint();
    }
    protected virtual void LoadEndPoint()
    {
        if (this.endPoint != null) return;
        this.endPoint = FindAnyObjectByType<CharacterAnimHandle>().transform;
        Debug.Log(transform.name + ": Load EndPoint!", gameObject);
    }
    protected virtual void LoadMinigunBulletZombieSpawner()
    {
        if (this.minigunBulletZombieSpawner != null) return;
        this.minigunBulletZombieSpawner = FindAnyObjectByType<MinigunBulletZombieSpawner>();
        Debug.Log(transform.name + ": LoadAbyssToxicSpawner");
    }
    protected virtual void LoadMinigunBulletZombie()
    {
        if (this.bulletMinigunPrefab != null) return;
        List<MinigunBulletZombie> allMyComponents = ComponentFinder.FindAllComponentsInScene<MinigunBulletZombie>();
        this.bulletMinigunPrefab = allMyComponents[0];
        Debug.Log(transform.name + ": LoadToxicAbyss");
    }
    protected override void Shooting()
    {
        StartCoroutine(this.ShootingRoutine());
    }
    private IEnumerator ShootingRoutine()
    {
        for (int i = 0; i < shootTime; i++)
        {
            MinigunBulletZombie newBullet = this.minigunBulletZombieSpawner.Spawn(bulletMinigunPrefab, listPositions[i % listPositions.Count].position);
            if (newBullet == null) yield break;
            newBullet.gameObject.GetComponent<MinigunBulletZombie>().ShootAt(endPoint.position + offSet);
            yield return new WaitForSeconds(1f);
        }
    }
}
