#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public static class TreeBrushToolEditor
{
    static bool isActive = false;
    static TreeBrushTool currentTool;

    static TreeBrushToolEditor()
    {
        Selection.selectionChanged += OnSelectionChanged;
        SceneView.duringSceneGui += OnSceneGUI;
    }

    static void OnSelectionChanged()
    {
        currentTool = null;
        isActive = false;

        if (Selection.activeGameObject != null)
        {
            currentTool = Selection.activeGameObject.GetComponent<TreeBrushTool>();
            isActive = currentTool != null;
        }
    }

    static void OnSceneGUI(SceneView sceneView)
    {
        if (!isActive || currentTool == null)
            return;

        Event e = Event.current;
        Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);

        if (!Physics.Raycast(ray, out RaycastHit hit)) return;

        Vector3 cursorPos = hit.point;

        if (e.shift)
        {
            // 🔴 Hiển thị vòng tròn đen nhạt tại vị trí con trỏ
            Handles.color = new Color(0f, 0f, 0f, 0.5f); // Màu đen, alpha 50%
            Handles.DrawSolidDisc(cursorPos, Vector3.up, currentTool.eraseRadius);

            // 🔁 Cuộn chuột thay đổi bán kính
            if (e.type == EventType.ScrollWheel)
            {
                float scroll = -e.delta.y * 0.1f;
                currentTool.eraseRadius = Mathf.Clamp(currentTool.eraseRadius + scroll, 0.5f, 50f);
                e.Use();
                SceneView.RepaintAll();
            }

            // 🧹 Click trái để xóa
            if (e.type == EventType.MouseDown && e.button == 0)
            {
                currentTool.EraseTrees(cursorPos);
                e.Use();
            }
        }
        else
        {
            // 🌱 Chế độ trồng cây
            if (e.type == EventType.MouseDown && e.button == 0 && !e.alt)
            {
                currentTool.SpawnTree(cursorPos);
                e.Use();
            }
        }
    }
}
#endif
