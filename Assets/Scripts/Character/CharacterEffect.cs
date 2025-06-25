using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterEffect : VuMonoBehaviour
{
    [SerializeField] private ParticleSystem _healingEffect;
    [SerializeField] private ParticleSystem _pickUpAmmour;


    public static Action HealingEffect;

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
}
