using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombiePoliceCtrl : EShooting
{
    [SerializeField] protected BulletInvisible bulletInvisible;
    [SerializeField] protected EnemyAIController enemyAIController;
    [SerializeField] protected int shootTime = 2;
    [SerializeField] protected float timeDelay = 0.1f;
    [SerializeField] protected float timeToNextShoot = 0f;

    [SerializeField] protected Transform gunFlash;
    [SerializeField] protected List<AudioClip> snd_shoot;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadBulletInvisible();
    }
    protected virtual void LoadBulletInvisible()
    {
        if (this.bulletInvisible != null) return;
        List<BulletInvisible> allMyComponents = ComponentFinder.FindAllComponentsInScene<BulletInvisible>();
        this.bulletInvisible = allMyComponents[0];
    }

    protected virtual void Shoot()
    {
        StartCoroutine(ShootingRoutine());
    }

    private IEnumerator ShootingRoutine()
    {
        for (int i = 0; i < shootTime; i++)
        {
            yield return new WaitForSeconds(this.timeDelay);
            this.Shooting(this.bulletInvisible, this.positionSpawn);
            if (this.newProjectitle == null) yield break;
            if (this.gunFlash != null) this.gunFlash.gameObject.SetActive(true);
            this.newProjectitle.GetComponent<BulletInvisible>().ShootAt(this.targetPosition.position);


            AudioClip audi = null;
            if (this.snd_shoot.Count != 0)
            {
                int random = Random.Range(0, snd_shoot.Count);
                if (this.snd_shoot[random] != null) audi = this.snd_shoot[random];
            }
            else audi = SoundFXManager.Instance.rifleShoot;
            if(audi != null) SoundFXManager.Instance.PlaySoundFXClip(audi, this.transform);


        }
        yield return new WaitForSeconds(this.timeToNextShoot);
        this.enemyAIController.EndAttack();
        if (this.gunFlash != null) this.gunFlash.gameObject.SetActive(false);
    }
}
