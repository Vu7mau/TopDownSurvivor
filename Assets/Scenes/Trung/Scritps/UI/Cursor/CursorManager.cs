using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class CursorManager : Singleton<CursorManager>
{
    [SerializeField] protected CursorIconsCtrl cursorIconsCtrl;
    public CursorIconsCtrl CursorIconsCtrl { get => this.cursorIconsCtrl; }
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Reset()
    {
        base.Reset();
        this.LoadCursorIconsCtrl();
    }
    protected virtual void LoadCursorIconsCtrl()
    {
        if (this.cursorIconsCtrl != null) return;
        this.cursorIconsCtrl = GetComponentInChildren<CursorIconsCtrl>();
    }


    public virtual void SetCurrentCursor(Texture2D cursor)
    {
        if (cursor != null)
        {
            Cursor.visible = true;
            Cursor.SetCursor(cursor, Vector2.zero, CursorMode.Auto);
        }
        else
        {
            Cursor.visible = false;
        }
    }
}
