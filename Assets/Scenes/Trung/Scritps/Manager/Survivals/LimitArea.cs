using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LimitArea : MonoBehaviour
{
    [SerializeField] protected Transform position;
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            //Debug.Log("Quái đã ra ngoài bản đồ");
            StartCoroutine(EnemySetRoutine(other));
        }
    }

    public IEnumerator EnemySetRoutine(Collider enemy)
    {
        enemy.GetComponent<NavMeshAgent>().enabled = false;
        yield return new WaitForSeconds(0.5f);
        enemy.transform.position = position.position;
        enemy.GetComponent<EnemyAIController>().SnapToNavMesh();
    }
}
