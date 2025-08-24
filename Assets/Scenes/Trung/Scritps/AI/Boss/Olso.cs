using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Olso : BossController
{
    [Header("Shoot 1")]
    [SerializeField] protected Projectitle minigunBullet;
    [SerializeField] protected Transform spawnPosition;
    [SerializeField] protected EShooting e_Shooting;
    [SerializeField] protected EffectFXSpawner effectFXSpawner;
    [SerializeField] protected EffectFX circleWarning;
    [SerializeField] protected List<AudioClip> snd_Shoot1s;

    [Space]
    [Header("Shoot 2")]
    [SerializeField] protected Projectitle rocketLight;
    [SerializeField] protected Transform spawnPosition2;
    [SerializeField] protected EffectFX circleWarning2;
    [SerializeField] protected AudioClip snd_shoot2_start;
    [SerializeField] protected AudioClip snd_shoot2_end;

    [Space]
    [Header("Jump Attackk")]
    [SerializeField] protected List<AudioClip> snd_jump_start;
    [SerializeField] protected AudioClip snd_jump_end;

    [Space]
    [Header("Death")]
    [SerializeField] protected List<AudioClip> snd_death;

    [SerializeField] protected float timeDelay_1 = 0.2f;
    [SerializeField] protected float timeDelay_2 = 1f;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadEShooting();
    }

    protected virtual void LoadEShooting()
    {
        if (this.e_Shooting != null) return;
        this.e_Shooting = GetComponent<EShooting>();
    }




    protected virtual void Shoot1()
    {
        StartCoroutine(Shoot1Routine());
    }
    private IEnumerator Shoot1Routine()
    {
        Vector3 tar = this.targetPosition.position;
        EffectFX circleWarning = this.effectFXSpawner.Spawn(this.circleWarning,tar + new Vector3(0,0.5f,0));
        if (circleWarning != null)
        {
            circleWarning.Scale(new Vector3(1.1f, 1.1f, 1.1f));
            circleWarning.Rotate(new Vector3(-90, 0, 0));
        }
        yield return new WaitForSeconds(this.timeDelay_1);
        this.e_Shooting.Shooting(this.minigunBullet, this.spawnPosition);
        if (this.isDead) yield break;
        if (this.e_Shooting.NewProjectitle == null) yield break;
        if(this.snd_Shoot1s.Count > 0)
        {
            int random = Random.Range(0, this.snd_Shoot1s.Count);
            SoundFXManager.Instance.PlaySoundFXClip(this.snd_Shoot1s[random],this.transform);
        }
        this.e_Shooting.NewProjectitle.GetComponent<Projectitle>().ShootAt(tar);
    }



    protected virtual void Shoot2()
    {
        StartCoroutine(Shoot2Routine());
    }
    private IEnumerator Shoot2Routine()
    {
        //EffectFX circleWarning = this.effectFXSpawner.Spawn(this.circleWarning, tar + new Vector3(0, 0.5f, 0));
        //if (circleWarning != null)
        //{
        //    circleWarning.Scale(new Vector3(1, 1, 1));
        //    circleWarning.Rotate(new Vector3(-90, 0, 0));
        //}
        //
        Vector3 tar = new Vector3(this.targetPosition.position.x, this.spawnPosition2.position.y, this.targetPosition.position.z);
        this.e_Shooting.Shooting(this.rocketLight, this.spawnPosition2);
        if (this.e_Shooting.NewProjectitle == null) yield break;
        if (this.isDead) yield break;
        if (this.snd_shoot2_start != null) SoundFXManager.Instance.PlaySoundFXClip(this.snd_shoot2_start, this.transform);
        float defaultSpeed = this.e_Shooting.NewProjectitle.Speed;
        this.e_Shooting.NewProjectitle.GetComponent<Projectitle>().SetVelocity(0);
        this.e_Shooting.NewProjectitle.GetComponentInChildren<Collider>().enabled = false;
        yield return new WaitForSeconds(this.timeDelay_2);
        if (this.snd_shoot2_end != null) SoundFXManager.Instance.PlaySoundFXClip(this.snd_shoot2_end, this.transform);
        this.e_Shooting.NewProjectitle.GetComponentInChildren<Collider>().enabled = true;
        this.e_Shooting.NewProjectitle.GetComponent<Projectitle>().SetVelocity(defaultSpeed);
        this.e_Shooting.NewProjectitle.GetComponent<Projectitle>().ShootAt(tar);
    }

    protected virtual void JumpParkourAttack()
    {
        EffectFX newCir;
        this.isMoving = false;
        this.enemyReferences.NavMeshAgent.enabled = false;
        if (this.isDead) return;
        transform.DOJump(this.targetPosition.position, 8f, 1, 1.2f)
        .OnStart(() =>
        {
            Vector3 pos = this.targetPosition.position;
            newCir = this.effectFXSpawner.Spawn(this.circleWarning2, pos + new Vector3(0, 0.5f, 0));
            newCir.Scale(new Vector3(2.35f,2.35f,2.35f));
            newCir.Rotate(new Vector3(-90f,0f,0f));
            if (this.snd_jump_start.Count > 0)
            {
                int random = Random.Range(0, this.snd_jump_start.Count);
                SoundFXManager.Instance.PlaySoundFXClip(this.snd_jump_start[random], this.transform);
            }
        })
        .OnComplete(() =>
        {
            if(this.snd_jump_end != null) SoundFXManager.Instance.PlaySoundFXClip(this.snd_jump_end, this.transform);
        });
    }

    public override void EndAttack()
    {
        base.EndAttack();
        this.enemyReferences.Animator.SetBool("attack", false);
    }

    protected override void Death()
    {
        base.Death();
        if (this.snd_death.Count > 0)
        {
            int random = Random.Range(0, this.snd_jump_start.Count);
            SoundFXManager.Instance.PlaySoundFXClip(this.snd_death[random], this.transform);
        }
    }
}
