using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class FindMissingScripts : MonoBehaviour
{
    [MenuItem("Tools/Find Missing Scripts/In Scene")]
    static void FindMissingInScene()
    {
        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();
        int count = 0;

        foreach (GameObject go in allObjects)
        {
            Component[] components = go.GetComponents<Component>();
            for (int i = 0; i < components.Length; i++)
            {
                if (components[i] == null)
                {
                    string path = GetGameObjectPath(go);
                    Debug.LogWarning($"[Scene] Missing script in: {path}", go);
                    count++;
                }
            }
        }

        Debug.Log($"[Scene] Total GameObjects with missing scripts: {count}");
    }

    [MenuItem("Tools/Find Missing Scripts/In Project Prefabs")]
    static void FindMissingInPrefabs()
    {
        string[] prefabGUIDs = AssetDatabase.FindAssets("t:Prefab");
        int totalMissing = 0;

        foreach (string guid in prefabGUIDs)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);

            if (prefab == null)
                continue;

            GameObject prefabInstance = PrefabUtility.LoadPrefabContents(path);
            Component[] components = prefabInstance.GetComponentsInChildren<Component>(true);

            foreach (Component comp in components)
            {
                if (comp == null)
                {
                    Debug.LogWarning($"[Prefab] Missing script in prefab: {path}", prefab);
                    totalMissing++;
                    break; // chỉ cần báo lỗi 1 lần cho mỗi prefab
                }
            }

            PrefabUtility.UnloadPrefabContents(prefabInstance);
        }

        Debug.Log($"[Prefab] Total prefabs with missing scripts: {totalMissing}");
    }

    static string GetGameObjectPath(GameObject obj)
    {
        string path = obj.name;
        Transform current = obj.transform;

        while (current.parent != null)
        {
            current = current.parent;
            path = current.name + "/" + path;
        }

        return path;
    }
}
