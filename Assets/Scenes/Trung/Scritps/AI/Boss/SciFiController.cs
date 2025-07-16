using System.Collections;
using System.Collections.Generic;
using Autodesk.Fbx;
using Unity.VisualScripting;
using UnityEngine;
using static Cinemachine.CinemachineImpulseManager.ImpulseEvent;

[RequireComponent(typeof(EShooting))]
public class SciFiController : BossController 
{
    [SerializeField] protected EShooting e_Shooting;

    [SerializeField] protected Transform playerPosition;


    [Space]
    [Header("Sound FX")]
    [Space]

    [SerializeField] protected List<AudioClip> snd_steps;
    [SerializeField] protected List<AudioClip> snd_attack1s;
    [SerializeField] protected AudioClip snd_attack2;
    [SerializeField] protected AudioClip snd_attack3;
    [SerializeField] protected AudioClip snd_attack4;
    //[SerializeField] protected AudioClip snd_attack5;
    [SerializeField] protected AudioClip snd_attack6;
    [SerializeField] protected List<AudioClip> snd_deaths;


    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadPlayerPosition();
        this.LoadEShooting();
    }
    protected virtual void LoadPlayerPosition()
    {
        if (this.playerPosition != null) return;
        this.playerPosition = FindAnyObjectByType<CharacterAnimHandle>().transform;
    }
    protected virtual void LoadEShooting()
    {
        if (this.e_Shooting != null) return;
        this.e_Shooting = GetComponentInChildren<EShooting>();
    }




    [Header("Hit Splash")]
    [SerializeField] protected HitSplash hitSplash;
    [SerializeField] protected Transform hitSplashPosition;

    protected virtual void Attack2()
    {
        this.e_Shooting.Shooting(hitSplash, hitSplashPosition);
        Vector3 dir = this.targetPosition.position - new Vector3(this.hitSplashPosition.position.x, this.targetPosition.position.y, this.hitSplashPosition.position.z);
        if (this.e_Shooting.NewProjectitle == null) return;
        this.e_Shooting.NewProjectitle.GetComponent<Projectitle>().SetDirection(dir);
        this.e_Shooting.NewProjectitle.gameObject.transform.rotation = this.transform.rotation;
    }

    [Space]
    [Header("Shooting")]
    [SerializeField] protected MinigunBullet minigunBullet;
    [SerializeField] protected Transform minigunBulletSpawnPosition;

    [SerializeField] protected float shootingTime = 3; // Số lần bắn
    [SerializeField] protected float timeDelay = 0.2f;

    protected virtual void Attack6()
    {
        StartCoroutine(ShootingRoutine());
    }
    private IEnumerator ShootingRoutine()
    {
        for (int i = 0; i < this.shootingTime; i++)
        {
            Vector3 target = this.targetPosition.position;
            yield return new WaitForSeconds(this.timeDelay);
            this.e_Shooting.Shooting(this.minigunBullet, this.minigunBulletSpawnPosition);
            if (this.e_Shooting.NewProjectitle == null) yield break;
            this.PlayerSFXAttack6();
            this.e_Shooting.NewProjectitle.GetComponent<Projectitle>().ShootAt(target);
            yield return new WaitForSeconds(this.timeDelay);
        }
        this.EndAttack();
    }

    //Sound FX
    protected virtual void OnStepFootSoundFX()
    {
        if (this.snd_steps.Count == 0) return;
        this.PlaySoundFX(this.snd_steps);
    }
    protected virtual void PlayerSFXAttack1()
    {
        if (this.snd_attack1s.Count == 0) return;
        this.PlaySoundFX(this.snd_attack1s);
    }
    protected virtual void PlayerSFXEDeath()
    {
        if (this.snd_deaths.Count == 0) return;
        this.PlaySoundFX(this.snd_deaths);
    }

    protected virtual void PlayerSFXAttack6()
    {
        if (this.snd_attack6 == null) return;
        SoundFXManager.Instance.PlaySoundFXClip(snd_attack6, transform, 1);
    }
    protected virtual void PlayerSFXAttack3()
    {
        if(this.snd_attack3 == null) return;
        SoundFXManager.Instance.PlaySoundFXClip(snd_attack3, transform, 1);
    }

    protected virtual void PlayerSFXDeath()
    {
        if (this.snd_deaths.Count > 0) return;
        this.PlaySoundFX(snd_deaths);
    }

    [SerializeField] protected EffectFXSpawner explosionSpawner;
    [SerializeField] protected Explosion explosion;
    [SerializeField] protected List<Transform> explosionPoss;
    [SerializeField] protected Transform fire;
    protected virtual void DestroyBossExplosion()
    {
        if(this.explosionSpawner == null) return;
        if(explosion == null) return;
        StartCoroutine(DestroyExplosionRoutine());
    }
    IEnumerator DestroyExplosionRoutine()
    {
        if (this.explosionPoss.Count == 0) yield break;
        for (int i = 0; i < this.explosionPoss.Count; i++)
        {
            this.explosionSpawner.Spawn(this.explosion, this.explosionPoss[i].position);
            yield return new WaitForSeconds(1);
        }
        yield return new WaitForSeconds(1f);
        EffectFX nbig = this.explosionSpawner.Spawn(this.explosion, transform.position);
        nbig.transform.localScale = new Vector3(3, 3, 3);
        fire.transform.gameObject.SetActive(true);
    }





    private void PlaySoundFX(List<AudioClip> sounds)
    {
        int random = Random.Range(0, sounds.Count);
        SoundFXManager.Instance.PlaySoundFXClip(sounds[random], transform, 1);
    }
}
