using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindTargets : VuMonoBehaviour
{
    [SerializeField] protected List<Transform> targetsNearest;
    public List<Transform> TargetsNearest { get => targetsNearest; }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!this.targetsNearest.Contains(other.transform))
            {
                CharacterDamageReceiver player = other.transform.GetComponentInChildren<CharacterDamageReceiver>();
                if (player != null)
                {
                    // Nếu máu của Player > 0 hoặc isDead = false thì thêm vào danh sách kẻ thù
                }
                this.targetsNearest.Add(other.transform);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        this.targetsNearest.Remove(other.transform);
    }
}
