using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AgentTypeDatabase", menuName = "NavMesh/AgentType Database")]
public class AgentTypeDatabase : ScriptableObject
{
    [Serializable]
    public class AgentTypeEntry
    {
        public string agentName;
        public int agentTypeID;
    }

    public List<AgentTypeEntry> agentTypes = new List<AgentTypeEntry>();

    public string GetNameByID(int id)
    {
        foreach (var entry in agentTypes)
            if (entry.agentTypeID == id)
                return entry.agentName;
        return null;
    }

    public int GetIDByName(string name)
    {
        foreach (var entry in agentTypes)
            if (entry.agentName == name)
                return entry.agentTypeID;
        return -1;
    }
}
