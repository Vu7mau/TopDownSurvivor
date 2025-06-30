using UnityEngine;

public class TreeBrushTool : MonoBehaviour
{
    public GameObject[] treePrefabs;
    [Range(0, 100)] public int selectedTreeIndex = 0;
    public float minScale = 0.8f;
    public float maxScale = 1.2f;

    [HideInInspector] public float eraseRadius = 2f;

    public void SpawnTree(Vector3 position)
    {
#if UNITY_EDITOR
        if (treePrefabs == null || treePrefabs.Length == 0)
            return;

        int index = Mathf.Clamp(selectedTreeIndex, 0, treePrefabs.Length - 1);
        GameObject prefab = treePrefabs[index];
        if (prefab == null)
        {
            Debug.LogWarning("TreeBrushTool: Selected prefab is null!");
            return;
        }

        GameObject tree = (GameObject)UnityEditor.PrefabUtility.InstantiatePrefab(prefab);
        tree.transform.position = position;
        tree.transform.rotation = Quaternion.Euler(0, Random.Range(0f, 360f), 0);
        float scale = Random.Range(minScale, maxScale);
        tree.transform.localScale = Vector3.one * scale;

        UnityEditor.Undo.RegisterCreatedObjectUndo(tree, "Paint Tree");
#endif
    }

    public void EraseTrees(Vector3 center)
    {
#if UNITY_EDITOR
        if (treePrefabs == null || treePrefabs.Length == 0) return;
        int index = Mathf.Clamp(selectedTreeIndex, 0, treePrefabs.Length - 1);
        GameObject targetPrefab = treePrefabs[index];
        if (targetPrefab == null) return;

        string targetName = targetPrefab.name;

        GameObject[] allTrees = GameObject.FindObjectsOfType<GameObject>();
        foreach (GameObject go in allTrees)
        {
            if (go.name.Contains(targetName)) // xóa dựa theo tên prefab
            {
                float distance = Vector3.Distance(go.transform.position, center);
                if (distance <= eraseRadius)
                {
                    UnityEditor.Undo.DestroyObjectImmediate(go);
                }
            }
        }
#endif
    }
}
