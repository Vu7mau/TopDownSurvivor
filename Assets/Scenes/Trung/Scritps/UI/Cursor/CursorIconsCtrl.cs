using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class CursorIconsCtrl : VuMonoBehaviour
{
    [SerializeField] protected CursorIcons cursorIcons;
    public CursorIcons CursorIcons { get => this.cursorIcons; }

    [SerializeField] private string soAddress = "Assets/Scenes/Trung/Scritps/UI/Cursor/CursorIcons.asset";

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadCursorIcons();
    }

    protected virtual void LoadCursorIcons()
    {
        Addressables.LoadAssetAsync<CursorIcons>(soAddress).Completed += handle =>
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                cursorIcons = handle.Result;
            }
            else
            {
                Debug.LogError("Không thể load CursorIcons từ Addressables.");
            }
        };
    }
}
