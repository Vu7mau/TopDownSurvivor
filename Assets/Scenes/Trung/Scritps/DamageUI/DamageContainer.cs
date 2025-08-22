using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageContainer : VuMonoBehaviour
{
    [SerializeField] protected List<DamageSender> damageSenders;
    public List<DamageSender> DamageSenders { get => this.damageSenders; set => this.damageSenders = value; }

    protected DamageSender currentDamageSender;
    public DamageSender CurrentDamageSender { get => this.currentDamageSender; }

    protected override void OnEnable()
    {
        this.damageSenders.Clear();
        CharacterEvents.OnDamageSourceListChanged += AddDamageSource;
    }
    protected override void OnDisable()
    {
        CharacterEvents.OnDamageSourceListChanged -= AddDamageSource;
    }

    protected virtual void UpdateCurrentDamageSource()
    {
        foreach(DamageSender sender in this.damageSenders)
        {
            if(sender.GetComponentInParent<RayCastWeapon>() != null)
            {
                if (sender.GetComponentInParent<RayCastWeapon>().IsWeaponActivate)
                {
                    this.currentDamageSender = sender;
                    CharacterEvents.OnDamageSourceChanged?.Invoke(sender);
                    return;
                }
            }
        }
    }

    public virtual void AddDamageSource(DamageSender damageSender)
    {
        this.damageSenders.Add(damageSender);
    }

    protected void FixedUpdate()
    {
        //this.UpdateCurrentDamageSource();
    }
}
