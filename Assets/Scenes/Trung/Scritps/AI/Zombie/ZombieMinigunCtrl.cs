using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ZombieMinigunCtrl : EShooting
{
    protected EnemyAIController enemyAIController;


    [SerializeField] protected List<Transform> listPositions;
    [SerializeField] protected MinigunBulletZombie minigunBullet;
    [SerializeField] protected int shootTime = 10;
    
    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadEnemyAIController();
    }

    protected virtual void LoadEnemyAIController()
    {
        if (this.enemyAIController != null) return;
        this.enemyAIController = GetComponent<EnemyAIController>();
    }
    protected virtual void MinigunShoot()
    {
        StartCoroutine(this.ShootingRoutine());
    }
    private IEnumerator ShootingRoutine()
    {
        for(int i = 0; i< shootTime; i++)
        {
            this.enemyAIController.IsLookAtTarget = true;
            int dem = Random.Range(0, this.listPositions.Count);
            yield return new WaitForSeconds(0.7f);
            this.Shooting(this.minigunBullet, this.listPositions[dem]);
            if (this.newProjectitle == null) yield break;
            this.newProjectitle.gameObject.GetComponent<MinigunBulletZombie>().ShootAt(this.targetPosition.position);
        }
        this.enemyAIController.EndAttack();
    }
}
