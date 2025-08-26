using UnityEngine;

[RequireComponent(typeof(Camera))]
public class MiniMapShaderOverride : MonoBehaviour
{
    public Shader replacementShader; // gán shader này trong Inspector

    void OnEnable()
    {
        if (replacementShader != null)
        {
            // Ép camera này render toàn bộ scene bằng replacementShader
            GetComponent<Camera>().SetReplacementShader(replacementShader, "RenderType");
        }
    }

    void OnDisable()
    {
        // Trả camera về trạng thái bình thường
        GetComponent<Camera>().ResetReplacementShader();
    }
}
