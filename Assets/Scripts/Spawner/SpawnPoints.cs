using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoints : VuMonoBehaviour
{
    [Header("SpawnPoints")]
    [SerializeField] protected List<Transform> points;

    protected override void LoadComponent()
    {

        this.LoadSpawnPoint();
    }
    protected virtual void LoadSpawnPoint()
    {
        if (points.Count>1) return;

        foreach (Transform point in this.transform)
        {
            this.points.Add(point);
        }
        Debug.Log("Load SpawnPoint Success at " + this.transform.name);
    }
}
