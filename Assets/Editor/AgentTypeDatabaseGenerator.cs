#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using System.Linq;

public static class AgentTypeDatabaseGenerator
{
    [MenuItem("Tools/NavMesh/Generate AgentType Database")]
    public static void Generate()
    {
        var db = ScriptableObject.CreateInstance<AgentTypeDatabase>();
        db.agentTypes.Clear();

        // Load NavMesh project settings asset
        var settingsObjects = AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/NavMeshAreas.asset");
        if (settingsObjects.Length == 0)
        {
            Debug.LogError("❌ Không tìm thấy NavMeshAreas.asset.");
            return;
        }

        var so = new SerializedObject(settingsObjects[0]);
        var settingsArray = so.FindProperty("m_Settings");
        var settingName = so.FindProperty("m_SettingNames");

        for (int i = 0; i < settingsArray.arraySize; i++)
        {
            var element = settingsArray.GetArrayElementAtIndex(i);
            int id = element.FindPropertyRelative("agentTypeID").intValue;
            string name = settingName.GetArrayElementAtIndex(i).stringValue;

            if (!string.IsNullOrEmpty(name))
            {
                db.agentTypes.Add(new AgentTypeDatabase.AgentTypeEntry
                {
                    agentName = name,
                    agentTypeID = id
                });
            }
        }

        // Tạo asset
        string path = "Assets/AgentTypeDatabase.asset";
        AssetDatabase.CreateAsset(db, path);
        AssetDatabase.SaveAssets();

        Debug.Log($"✅ Đã tạo AgentTypeDatabase tại: {path} với {db.agentTypes.Count} agent types.");
    }
}
#endif
