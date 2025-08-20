using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ZombieMinigunCtrl : EShooting
{
    protected EnemyAIController enemyAIController;


    [SerializeField] protected List<Transform> listPositions;
    [SerializeField] protected AudioClip snd_shoot;
    [SerializeField] protected Projectitle minigunBullet;
    [SerializeField] protected int shootTime = 10;
    [SerializeField] protected float delayTime = 0.7f;
    
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
        //if (this.snd_shoot != null) SoundEnemyManager.Instance.PlayEnemySoundFXClip(this.snd_shoot, this.transform, true, 1f);
        for (int i = 0; i< shootTime; i++)
        {
            this.enemyAIController.IsLookAtTarget = true;
            int dem = Random.Range(0, this.listPositions.Count);
            yield return new WaitForSeconds(this.delayTime);
            this.Shooting(this.minigunBullet, this.listPositions[dem]);
            if (this.newProjectitle == null) yield break;
            float defaultSpeed = this.newProjectitle.Speed;
            this.newProjectitle.gameObject.GetComponent<Projectitle>().SetVelocity(0f);
            this.newProjectitle.gameObject.GetComponent<Projectitle>().SetDirection(Vector3.zero);
            this.newProjectitle.gameObject.GetComponentInChildren<TrailRenderer>().enabled = false;
            yield return new WaitForSeconds(0.1f);
            SoundFXManager.Instance.PlaySoundFXClip(this.snd_shoot, this.transform);
            this.newProjectitle.gameObject.GetComponentInChildren<TrailRenderer>().enabled = true;
            this.newProjectitle.gameObject.GetComponent<Projectitle>().SetVelocity(defaultSpeed);
            this.newProjectitle.gameObject.GetComponent<Projectitle>().ShootAt(this.targetPosition.position);
        }
        this.enemyAIController.EndAttack();
    }
}
