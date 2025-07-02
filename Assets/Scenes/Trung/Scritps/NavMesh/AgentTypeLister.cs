using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class AgentTypeLister
{
    [MenuItem("Tools/List NavMesh Agent Types")]
    public static void ListAgentTypes()
    {
        int count = NavMesh.GetSettingsCount();
        for (int i = 0; i < count; i++)
        {
            var settings = NavMesh.GetSettingsByIndex(i);
            string name = GetAgentTypeNameByID(settings.agentTypeID);
            Debug.Log($"Agent ID: {settings.agentTypeID}, Name: {name}");
        }
    }

    // Trích xuất tên agent type từ NavMeshSettings.asset
    static string GetAgentTypeNameByID(int id)
    {
        var settingsObjs = AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/NavMeshAreas.asset");

        foreach (var obj in settingsObjs)
        {
            if (obj == null) continue;

            SerializedObject so = new SerializedObject(obj);
            SerializedProperty settingsArray = so.FindProperty("m_Settings");

            if (settingsArray != null && settingsArray.isArray)
            {
                for (int i = 0; i < settingsArray.arraySize; i++)
                {
                    SerializedProperty element = settingsArray.GetArrayElementAtIndex(i);
                    var idProp = element.FindPropertyRelative("agentTypeID");
                    if (idProp != null && idProp.intValue == id)
                    {
                        var nameProp = element.FindPropertyRelative("name");
                        if (nameProp != null)
                            return nameProp.stringValue;
                    }
                }
            }
        }

        return "Unknown";
    }
}
