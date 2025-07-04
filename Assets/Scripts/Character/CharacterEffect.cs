using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterEffect : VuMonoBehaviour
{
    [SerializeField] private ParticleSystem _healingEffect;
    [SerializeField] private ParticleSystem _pickUpAmmour;


    [SerializeField] private Transform light;


    public static Action HealingEffect;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadLightObject();
    }
    private void LoadLightObject()
    {
        if (light != null) return;

        light = this.transform.Find("Spot Light").transform;
    }
    protected override void OnEnable()
    {
        HealingEffect += PlayHealingEffect;
    }
    protected override void OnDisable()
    {
        HealingEffect += PlayHealingEffect;
    }

    private void PlayHealingEffect()
    {
        if (_healingEffect == null) return;

        _healingEffect.gameObject.SetActive(true);
        _healingEffect.Play();
    }

    public void TurnOnLight()
    {
        if (light==null) return;

        light.gameObject.SetActive(true);
    }
    public void TurnOffLight()
    {
        if (light==null) return;

        light.gameObject.SetActive(false);
    }
}
