using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameObjectCustom
{
    public static T FindFirstComponentInChildren<T>(this Transform parent) where T : Component
    {
        foreach (Transform child in parent)
        {
            // Kiểm tra trực tiếp ở child
            T comp = child.GetComponent<T>();
            if (comp != null)
                return comp;

            // Nếu chưa có thì đệ quy tiếp tục tìm ở cấp con
            comp = child.FindFirstComponentInChildren<T>();
            if (comp != null)
                return comp;
        }
        return null;
    }

    public static T FindParentComponent<T>(GameObject child) where T : Component
    {
        if (child == null) return null;

        Transform current = child.transform.parent;
        while (current != null)
        {
            T comp = current.GetComponent<T>();
            if (comp != null)
                return comp;

            current = current.parent;
        }

        return null; // Không tìm thấy
    }
}
