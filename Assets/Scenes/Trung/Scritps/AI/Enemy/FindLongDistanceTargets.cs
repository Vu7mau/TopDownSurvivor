using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindLongDistanceTargets : MonoBehaviour
{
    [SerializeField] protected List<Transform> targetsNearest;
    public List<Transform> TargetsNearest { get => targetsNearest; }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!this.targetsNearest.Contains(other.transform)) this.targetsNearest.Add(other.transform);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        this.targetsNearest.Remove(other.transform);
    }
}
