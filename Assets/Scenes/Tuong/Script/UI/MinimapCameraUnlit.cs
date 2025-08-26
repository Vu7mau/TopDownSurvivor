using UnityEngine;

[RequireComponent(typeof(Camera))]
public class MiniMapOverride : MonoBehaviour
{
    public Shader minimapShader;

    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();

        if (minimapShader != null)
        {
            // ép tất cả object render bằng shader này, không quan tâm material gốc
            cam.SetReplacementShader(minimapShader, "");
        }
    }

    void OnDisable()
    {
        if (cam != null)
        {
            // khôi phục lại như cũ
            cam.ResetReplacementShader();
        }
    }
}
