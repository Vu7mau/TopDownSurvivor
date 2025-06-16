using UnityEngine;
using UnityEngine.AI;

public class EnemyFollow : MonoBehaviour
{
    public Transform target;              // Player
    public float chaseRange = 10f;        // Khoảng cách bắt đầu đuổi
    public float stopDistance = 1.5f;     // Dừng khi gần người chơi

    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        if (target == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            Debug.Log("phat hien");
            if (player != null)
                target = player.transform;
        }
    }

    void Update()
    {
        if (target == null) return;

        float distance = Vector3.Distance(transform.position, target.position);

        if (distance <= chaseRange)
        {
            agent.SetDestination(target.position);

            // Dừng lại nếu quá gần
            if (distance <= stopDistance)
            {
                agent.isStopped = true;
            }
            else
            {
                agent.isStopped = false;
            }
        }
        else
        {
            agent.isStopped = true;
        }
    }
}
