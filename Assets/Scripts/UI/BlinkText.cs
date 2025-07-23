using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class BlinkText : VuMonoBehaviour
{

    public float blinkSpeed = 2f;
    public float minAlpha = 0.2f;
    public float maxAlpha = 1f;

   [SerializeField] private TMP_Text tmpText;
    private float currentAlpha;
    private bool fadingOut = true;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        tmpText = GetComponent<TMP_Text>();
    }
    protected override void Awake()
    {
        currentAlpha = maxAlpha;
    }

    private void Update()
    {
        // Tính alpha mới
        float delta = blinkSpeed * Time.deltaTime;
        if (fadingOut)
        {
            currentAlpha -= delta;
            if (currentAlpha <= minAlpha)
            {
                currentAlpha = minAlpha;
                fadingOut = false;
            }
        }
        else
        {
            currentAlpha += delta;
            if (currentAlpha >= maxAlpha)
            {
                currentAlpha = maxAlpha;
                fadingOut = true;
            }
        }

        UpdateVertexAlpha(currentAlpha);
    }

    private void UpdateVertexAlpha(float alpha)
    {
        tmpText.ForceMeshUpdate();
        var textInfo = tmpText.textInfo;

        for (int i = 0; i < textInfo.characterCount; i++)
        {
            if (!textInfo.characterInfo[i].isVisible)
                continue;

            int matIndex = textInfo.characterInfo[i].materialReferenceIndex;
            int vertIndex = textInfo.characterInfo[i].vertexIndex;

            Color32[] colors = textInfo.meshInfo[matIndex].colors32;

            for (int j = 0; j < 4; j++) // 4 vertex của mỗi ký tự
            {
                var originalColor = colors[vertIndex + j];
                colors[vertIndex + j] = new Color32(originalColor.r, originalColor.g, originalColor.b, (byte)(alpha * 255));
            }
        }

        // Apply lại màu cho text
        for (int i = 0; i < textInfo.meshInfo.Length; i++)
        {
            textInfo.meshInfo[i].mesh.colors32 = textInfo.meshInfo[i].colors32;
            tmpText.UpdateGeometry(textInfo.meshInfo[i].mesh, i);
        }
    }

}
