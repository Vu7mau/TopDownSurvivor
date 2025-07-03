using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.AI;

public class AgentTypeSwitcher : MonoBehaviour
{
    public float raycastDistance = 2f;

    [SerializeField] AgentTypeDatabase agentTypeDB;
    protected EnemyAI enemyAI;
    private NavMeshAgent agent;

    // Tạo danh sách ánh xạ giữa tên và ID agent
    private Dictionary<string, int> agentTypeLookup = new Dictionary<string, int>();


    protected void Awake()
    {
        AddNavMeshSurfaceTag();
        this.LoadAgentTypeDatabase();
        //CacheAgentTypeIDs(); // Khởi tạo ánh xạ
    }
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        enemyAI = GetComponent<EnemyAI>();
    }

    void Update()
    {
        DetectSurfaceBelow();
        //CheckNavMeshBelow();
    }

    protected virtual void LoadAgentTypeDatabase()
    {
        Addressables.LoadAssetAsync<AgentTypeDatabase>("Assets/AgentTypeDatabase.asset").Completed += handle =>
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                agentTypeDB = handle.Result;
                Debug.Log("✅ Loaded AgentTypeDatabase");
                
                // Bắt đầu sử dụng agentTypeDB ở đây
            }
            else
            {
                Debug.LogError("❌ Failed to load AgentTypeDatabase from Addressables.");
            }
        };
    }

    void CacheAgentTypeIDs()
    {
        int count = this.agentTypeDB.agentTypes.Count;
        for (int i = 0; i < count; i++)
        {
            var settings = this.agentTypeDB.agentTypes[i].agentTypeID;
            // Tự định nghĩa ánh xạ agentTypeName → ID
            // Bạn cần cập nhật tên này đúng với Project Settings > Navigation > Agents
            switch (settings)
            {
                case 0: agentTypeLookup["map1"] = 0; break;
                case 658490984: agentTypeLookup["map2"] = 658490984; break;
                case -629701670: agentTypeLookup["map0"] = -629701670; break;
                    // Thêm agent khác ở đây nếu có
            }
        }
    }

    private void AddNavMeshSurfaceTag()
    {
        var navSurfaces = FindObjectsOfType<NavMeshSurface>(true); // include inactive objects

        foreach (var surface in navSurfaces)
        {
            if (surface.GetComponentInChildren<NavMeshSurface>() != null)
            {
                surface.gameObject.AddComponent<NavMeshSurfaceTag>();
                Debug.Log("Added to: " + surface.name);
            }
        }

        Debug.Log("Finished adding script to all NavMeshSurfaces.");
    }

    public Vector3 offset;

    //public float checkDistance = 1.0f;
    //public LayerMask navMeshLayerMask;
    //public int agentTypeID;


    //void CheckNavMeshBelow()
    //{
    //    Vector3 origin = transform.position + Vector3.up * 0.5f;
    //    Vector3 direction = Vector3.down;

    //    if (NavMesh.SamplePosition(origin, out NavMeshHit hit, checkDistance, NavMesh.AllAreas))
    //    {
    //        // Kiểm tra agent type nếu cần
    //        if (agentTypeID == -1 || NavMesh.GetAreaFromName("Walkable") == hit.mask)
    //        {
    //            Debug.Log("Có NavMesh phía dưới tại: " + hit.position);
    //        }
    //    }
    //    else
    //    {
    //        Debug.Log("Không có NavMesh bên dưới.");
    //    }
    //}



    void DetectSurfaceBelow()
    {
        Ray ray = new Ray(transform.position + offset, Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit hit, raycastDistance))
        {
            Debug.Log("Hit: " + hit.transform.name);
            var surfaceTag = hit.collider.GetComponentInChildren<NavMeshSurfaceTag>();
            if (surfaceTag != null)
            {
                int newTypeID = this.agentTypeDB.GetIDByName(surfaceTag.agentTypeName);
                if (agent.agentTypeID != newTypeID)
                {
                    SwitchAgentType(newTypeID);
                }
            }
        }
    }

    void SwitchAgentType(int newTypeID)
    {
        Debug.Log($"Switching agent type to ID {newTypeID}");
        agent.agentTypeID = newTypeID;
        this.enemyAI.IsMoving = true;

        //if (agent.isOnNavMesh)
        //{
        //    agent.ResetPath();
        //}
        //else
        //{
        //    NavMeshHit navHit;
        //    if (NavMesh.SamplePosition(transform.position, out navHit, 2f, NavMesh.AllAreas))
        //    {
        //        agent.Warp(navHit.position); // Đặt lại lên NavMesh phù hợp
        //    }
        //}
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;

        Vector3 origin = transform.position + offset;
        Vector3 direction = Vector3.down * raycastDistance;

        Gizmos.DrawLine(origin, origin + direction);
        Gizmos.DrawSphere(origin, 0.05f); // Vị trí bắt đầu ray
    }
}
