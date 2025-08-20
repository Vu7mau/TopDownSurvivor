using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] protected List<AudioClip> snd_deaths;


    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadPlayerPosition();
        this.LoadEShooting();
        this.LoadEffectFXSpawner();
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



    [Space]
    [Space]
    [Space]
    [Header("Attack 2")]
    [SerializeField] protected Projectitle rocketLight;
    [SerializeField] protected List<Transform> rocketLightPosition;
    [SerializeField] protected List<Projectitle> rocketLightObj;
    [SerializeField] protected float timeDelayAttack2 = 0.1f;
    [SerializeField] protected float defaultSpeed = 50f;
    [SerializeField] protected List<AudioClip> snd_shoot2_start;

    [SerializeField] protected List<AudioClip> snd_shoot6_start;
    [SerializeField] protected List<AudioClip> snd_shoot6_end;
    [SerializeField] protected Vector3 rocketLightScale;


    protected override void OnEnable()
    {
        base.OnEnable();
        this.ResetScifi();

    }
    protected virtual void ResetScifi()
    {
        this.fire.transform.gameObject.SetActive(false);
    }
    protected virtual void Attack2()
    {
        StartCoroutine(Shoot2Routine());
        //this.e_Shooting.Shooting(hitSplash, hitSplashPosition);
        //Vector3 dir = this.targetPosition.position - new Vector3(this.hitSplashPosition.position.x, this.targetPosition.position.y, this.hitSplashPosition.position.z);
        //if (this.e_Shooting.NewProjectitle == null) return;
        //this.e_Shooting.NewProjectitle.GetComponent<Projectitle>().SetDirection(dir);
        //this.e_Shooting.NewProjectitle.gameObject.transform.rotation = this.transform.rotation;
    }

    private IEnumerator Shoot2Routine()
    {
        if (this.rocketLightPosition.Count == 0) yield break;
        for (int i = 0; i < this.rocketLightPosition.Count; i++)
        {
            this.e_Shooting.Shooting(this.rocketLight, this.rocketLightPosition[i]);
            if (this.e_Shooting.NewProjectitle == null) yield break;

            Projectitle newRocket = this.e_Shooting.NewProjectitle;
            if(this.rocketLightScale != Vector3.zero)
            {
                newRocket.transform.localScale = this.transform.localScale;
            }
            this.rocketLightObj.Add(newRocket);
            //if (this.snd_shoot2_start != null) SoundFXManager.Instance.PlaySoundFXClip(this.snd_shoot2_start, this.transform);
            newRocket.GetComponentInChildren<Projectitle>().SetVelocity(0);
            newRocket.GetComponentInChildren<Collider>().enabled = false;
            yield return null;
        }
        if (this.snd_shoot2_start.Count > 0)
        {
            int random = Random.Range(0, this.snd_shoot2_start.Count);
            SoundFXManager.Instance.PlaySoundFXClip(this.snd_shoot2_start[random], this.transform);
        }

        if (this.rocketLightObj.Count == 0) yield break;
        Vector3 direct = (this.targetPosition.position - this.transform.position).normalized;

        yield return new WaitForSeconds(this.timeDelayAttack2);
        if (this.isDead) yield break;

        for (int i = 0; i < this.rocketLightObj.Count; i++)
        {
            Vector3 tar = direct;
            //if (this.snd_shoot2_end != null) SoundFXManager.Instance.PlaySoundFXClip(this.snd_shoot2_end, this.transform);
            rocketLightObj[i].GetComponentInChildren<Collider>().enabled = true;
            rocketLightObj[i].GetComponentInChildren<Projectitle>().SetVelocity(this.defaultSpeed);
            rocketLightObj[i].GetComponentInChildren<Projectitle>().SetDirection(tar);
        }
        if (this.snd_shoot6_end.Count > 0)
        {
            int random = Random.Range(0, this.snd_shoot6_end.Count);
            SoundFXManager.Instance.PlaySoundFXClip(this.snd_shoot6_end[random], this.transform);
        }
        this.rocketLightObj.Clear();
    }



    [Space]
    [Space]
    [Space]
    [Header("Attack 6")]
    [SerializeField] protected MinigunBullet minigunBullet;
    [SerializeField] protected Transform minigunBulletSpawnPosition;

    [SerializeField] protected float shootingTime = 3; // Số lần bắn
    [SerializeField] protected float timeDelay = 0.2f;

    [SerializeField] protected EffectFX circleWarning6;
    [SerializeField] protected EffectFXSpawner effectFXSpawner;

    [SerializeField] protected AudioClip snd_attack6;
    protected virtual void LoadEffectFXSpawner()
    {
        if (this.effectFXSpawner != null) return;
        this.effectFXSpawner = FindAnyObjectByType<EffectFXSpawner>();
    }

    protected virtual void Attack6()
    {
        StartCoroutine(ShootingRoutine());
    }
    private IEnumerator ShootingRoutine()
    {
        if (this.snd_shoot6_start.Count > 0)
        {
            int random = Random.Range(0, this.snd_shoot6_start.Count);
            SoundFXManager.Instance.PlaySoundFXClip(this.snd_shoot6_start[random], this.transform);
        }
        for (int i = 0; i < this.shootingTime; i++)
        {
            this.LookAtTartgetPlease();
            Vector3 target = this.targetPosition.position;


            yield return new WaitForSeconds(0.1f);

            this.DontLookAtTarget();
            if (this.effectFXSpawner != null && this.circleWarning6 != null)
            {
                EffectFX newCirWarning = this.effectFXSpawner.Spawn(this.circleWarning6, target + new Vector3(0, 0.4f, 0));
                newCirWarning.Rotate(new Vector3(-90, 0, 0));
                Vector3 scale = this.circleWarning6.transform.localScale;
                newCirWarning.Scale(scale);
            }




            yield return new WaitForSeconds(this.timeDelay);

            if (this.isDead) yield break;
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
    protected virtual void PlayerSFXAttack4()
    {
        if (this.snd_attack4 == null) return;
        SoundFXManager.Instance.PlaySoundFXClip(snd_attack4, transform, 1);
    }

    protected virtual void SFXDeath()
    {
        if (this.snd_deaths.Count == 0) return;
        this.PlaySoundFX(snd_deaths);
    }


    [Space]
    [Space]
    [Space]
    [Header("Death")]
    [SerializeField] protected Explosion explosion;
    [SerializeField] protected List<Transform> explosionPoss;
    [SerializeField] protected Transform fire;
    [SerializeField] protected AudioClip snd_shutdownRobot;

    private void Distribution()
    {
        StartCoroutine(DestroyExplosionRoutine());
    }
    IEnumerator DestroyExplosionRoutine()
    {
        //if (this.explosionPoss.Count == 0) yield break;
        //for (int i = 0; i < this.explosionPoss.Count; i++)
        //{
        //    EffectFX nsmall =  this.effectFXSpawner.Spawn(this.explosion, this.explosionPoss[i].position);
        //    nsmall.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        //    yield return new WaitForSeconds(1);
        //}
        //yield return new WaitForSeconds(1f);
        yield return new WaitForSeconds(0);
        EffectFX nbig = this.effectFXSpawner.Spawn(this.explosion, transform.position);
        nbig.transform.localScale = new Vector3(3, 3, 3);
        fire.transform.gameObject.SetActive(true);
    }
    private void Shutdown()
    {
        if(this.snd_shutdownRobot == null) return;


        SoundFXManager.Instance.PlaySoundFXClip(this.snd_shutdownRobot, this.transform);
    }




    private void PlaySoundFX(List<AudioClip> sounds)
    {
        int random = Random.Range(0, sounds.Count);
        SoundFXManager.Instance.PlaySoundFXClip(sounds[random], transform, 1);
    }
}
