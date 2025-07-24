using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class SpiderTiny : Zombie_FireFighterCtrl
{
    [SerializeField] private Renderer meshRenderer;
    [SerializeField] private Color flashColor = Color.red;
    [SerializeField] private float flashDuration = 0.1f;
    [SerializeField] private int flashCount = 5;

    private Color originalColor;
    private Material instanceMaterial;
    private Coroutine flashRoutine;

    protected override void OnEnable()
    {
        base.OnEnable();
        this.StartSpawnFlash();
    }

    protected override void Awake()
    {
        base.Awake();
        // Tạo bản sao của material để không ảnh hưởng prefab
        instanceMaterial = meshRenderer.material;
        originalColor = instanceMaterial.color;
    }

    public void StartSpawnFlash()
    {
        if (flashRoutine != null) StopCoroutine(flashRoutine);
        flashRoutine = StartCoroutine(FlashSpawnCoroutine());
    }

    public void StartFastFlashAndExplode()
    {
        if (flashRoutine != null) StopCoroutine(flashRoutine);
        flashRoutine = StartCoroutine(FlashAndExplodeCoroutine());
    }

    private IEnumerator FlashSpawnCoroutine()
    {
        while (true)
        {
            instanceMaterial.color = flashColor;
            yield return new WaitForSeconds(0.2f);
            instanceMaterial.color = originalColor;
            yield return new WaitForSeconds(0.8f); // Tổng 1s
        }
    }

    private IEnumerator FlashAndExplodeCoroutine()
    {
        int flashCount = this.flashCount;
        for (int i = 0; i < flashCount; i++)
        {
            instanceMaterial.color = flashColor;
            yield return new WaitForSeconds(0.1f);
            instanceMaterial.color = originalColor;
            yield return new WaitForSeconds(0.1f);
        }
        this.Explode();
    }
}
