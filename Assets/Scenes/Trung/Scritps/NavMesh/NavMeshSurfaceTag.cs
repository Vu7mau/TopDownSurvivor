using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class NavMeshSurfaceTag : VuMonoBehaviour
{
    [SerializeField] AgentTypeDatabase agentTypeDB;
    [SerializeField] protected NavMeshSurface navMeshSurface;

    public string agentTypeName;

    protected override void Awake()
    {
        base.Awake();

    }

    protected override void Start()
    {
        base.Start();
        this.LoadAgentTypeDatabase();
    }
    protected virtual void LoadNavMeshSurface()
    {
        navMeshSurface = this.GetComponent<NavMeshSurface>();
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadNavMeshSurface();
    }
    protected virtual void LoadAgentTypeDatabase()
    {
        Addressables.LoadAssetAsync<AgentTypeDatabase>("Assets/AgentTypeDatabase.asset").Completed += handle =>
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                agentTypeDB = handle.Result;
                Debug.Log("✅ Loaded AgentTypeDatabase");
                foreach (var t in this.agentTypeDB.agentTypes)
                {
                    if (t.agentTypeID == this.navMeshSurface.agentTypeID)
                    {
                        this.agentTypeName = t.agentName;
                        break;
                    }
                }
                // Bắt đầu sử dụng agentTypeDB ở đây
            }
            else
            {
                Debug.LogError("❌ Failed to load AgentTypeDatabase from Addressables.");
            }
        };
    }
}
