using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AINavMesh : MonoBehaviour
{
    private NavMeshAgent agent;
    private void OnEnable()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.enabled = true;
    }
    private void OnDisable()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.enabled = false;
    }
}
