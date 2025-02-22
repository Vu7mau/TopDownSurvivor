using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : VuMonoBehaviour
{
    public static CursorManager Instance;
    // [SerializeField] protected Texture2D cursorTexture;
    //[SerializeField] protected float blinkInterval = 0.1f;
    //[SerializeField] protected bool isReloading = false;
    [SerializeField] protected Texture2D cursorTexture; // Ảnh con trỏ
    [SerializeField] protected Material cursorMaterial; // Material có Shader Graph
    [SerializeField] protected Color reloadColor = Color.red; // Màu khi thay đạn
    [SerializeField] protected Color originalColor; // Lưu màu gốc


    protected override void Awake()
    {
        base.Awake();
        if (Instance == null)
            Instance = this;
    }


  protected override  void Start()
    {
        if (cursorMaterial != null)
        {
            originalColor = cursorMaterial.GetColor("_ColorTint");
        }
        SetCursor(cursorTexture);
    }

    void SetCursor(Texture2D texture)
    {
        Cursor.SetCursor(texture, Vector2.zero, CursorMode.Auto);
    }

    public void StartReloadAnimation(float reloadTime)
    {
        if (cursorMaterial != null)
        {
            cursorMaterial.SetColor("_ColorTint", reloadColor);
            cursorTexture = cursorMaterial.GetTexture("Cursor") as Texture2D;
            Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.Auto);
            Invoke("ResetCursorColor", reloadTime);
        }
    }

    void ResetCursorColor()
    {
        if (cursorMaterial != null)
        {
            cursorMaterial.SetColor("_ColorTint", originalColor);
          //  Texture2D texture = cursorMaterial.GetTe("Cursor");
            Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.Auto);
        }
    }
}
