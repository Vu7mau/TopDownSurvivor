using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockBack : MonoBehaviour
{
    [SerializeField] private Material hurtMaterial;
    [SerializeField] private float time;
    [SerializeField] private Color materialColor;
    [SerializeField] private bool isHaveHurtEffect = true;
    private Material defaultMaterial;
    private SkinnedMeshRenderer meshRenderer;
    private void Awake()
    {
        meshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
    }
    private void Start()
    {
        defaultMaterial = meshRenderer.material;
        hurtMaterial.color = new Color(materialColor.r, materialColor.g, materialColor.b,materialColor.a);
    }
    public IEnumerator MaterialRoutine()
    {
        if (isHaveHurtEffect)
        {
            yield return new WaitForSeconds(time);
            meshRenderer.material = hurtMaterial;
            yield return new WaitForSeconds(time);
            meshRenderer.material = defaultMaterial;
        }
        
    }
}
