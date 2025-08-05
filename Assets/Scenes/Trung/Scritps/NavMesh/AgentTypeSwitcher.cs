using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Unity.AI.Navigation;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class AgentTypeSwitcher : MonoBehaviour
{
    public float raycastDistance = 2f;

    [SerializeField] private AgentTypeDatabase agentTypeDB;
    private NavMeshAgent agent;
    private EnemyAI enemyAI;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        enemyAI = GetComponent<EnemyAI>();
    }

    private void Start()
    {
#if UNITY_EDITOR
        LoadAgentTypeDatabaseAndAddSurfaces();
#endif
    }

#if UNITY_EDITOR
    [ContextMenu("Thêm Enemy NavMeshSurface ( chỉ trong chế độ Editor)")]
    private void AddEnemyNavMeshSurfaces()
    {
        if (agentTypeDB == null)
        {
            Debug.LogError("agentTypeDB chưa được load. Không thể thêm NavMeshSurface cho Enemy.");
            return;
        }

        var allSurfaces = FindObjectsOfType<NavMeshSurface>(true);

        foreach (var surface in allSurfaces)
        {
            if (surface.defaultArea == 0)
            {
                var go = surface.gameObject;

                bool hasEnemySurface = go.GetComponents<NavMeshSurface>()
                    .Any(s => NavMesh.GetSettingsNameFromID(s.agentTypeID) == "Enemy");

                if (!hasEnemySurface)
                {
                    var enemySurface = go.AddComponent<NavMeshSurface>();
                    enemySurface.agentTypeID = agentTypeDB.GetIDByName("Enemy");
                    enemySurface.overrideTileSize = surface.overrideTileSize;
                    enemySurface.tileSize = surface.tileSize;
                    enemySurface.overrideVoxelSize = surface.overrideVoxelSize;
                    enemySurface.voxelSize = surface.voxelSize;
                    enemySurface.collectObjects = CollectObjects.Children;
                    enemySurface.BuildNavMesh();

                    Debug.Log($"✅ Thêm Enemy NavMeshSurface vào {go.name}");
                }
            }
        }

        Debug.Log("✅ Hoàn tất thêm Enemy NavMeshSurface.");
    }

    private void LoadAgentTypeDatabaseAndAddSurfaces()
    {
        Addressables.LoadAssetAsync<AgentTypeDatabase>("Assets/AgentTypeDatabase.asset").Completed += handle =>
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                agentTypeDB = handle.Result;
                Debug.Log("✅ Loaded AgentTypeDatabase từ Addressables");

                AddEnemyNavMeshSurfaces(); // Gọi sau khi đã load xong
            }
            else
            {
                Debug.LogError("❌ Không thể load AgentTypeDatabase từ Addressables.");
            }
        };
    }
#endif
}

