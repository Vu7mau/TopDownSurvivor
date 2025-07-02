using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

[ExecuteAlways]
public class NavMeshSurfaceGizmo : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        var surfaces = FindObjectsOfType<NavMeshSurface>(true);
        foreach (var surface in surfaces)
        {
            if (surface == null) continue;

            Collider[] colliders = surface.GetComponentsInChildren<Collider>(true);
            if (colliders.Length == 0)
            {
                // Đặt màu cho Gizmo
                Gizmos.color = Color.red;

                // Lấy bounds để vẽ
                var renderer = surface.GetComponent<Renderer>();
                if (renderer != null)
                {
                    Gizmos.DrawWireCube(renderer.bounds.center, renderer.bounds.size);
                }
                else
                {
                    // Nếu không có renderer, vẽ hộp 1x1x1 tại vị trí
                    Gizmos.DrawWireCube(surface.transform.position, Vector3.one);
                }

                // Ghi nhãn
                GUIStyle style = new GUIStyle();
                style.normal.textColor = Color.red;
#if UNITY_EDITOR
                UnityEditor.Handles.Label(surface.transform.position + Vector3.up * 1.5f, surface.name + " (No Collider)", style);
#endif
            }
        }
    }
}
