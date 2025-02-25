using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveParentObj : MonoBehaviour
{
    [SerializeField] private float timeToDelete;
    [SerializeField] private GameObject childParent;
    private bool isActive = false;
    private void OnEnable()
    {
        childParent.SetActive(true);
        isActive = true;
        if (childParent != null && isActive)
        {
            StartCoroutine(ActiveChildRoutine());
        }
    }
    private void OnDisable()
    {
        isActive = false;
    }
    private IEnumerator ActiveParentRoutine()
    {
        yield return new WaitForSeconds(timeToDelete);
        gameObject.SetActive(false);
    }
    private IEnumerator ActiveChildRoutine()
    {
        yield return new WaitUntil(() => !childParent.activeInHierarchy);
        StartCoroutine(ActiveParentRoutine());
    }
}
