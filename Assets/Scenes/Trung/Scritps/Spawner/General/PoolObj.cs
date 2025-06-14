using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class ComponentFinder
{
    public static List<T> FindAllComponentsInScene<T>() where T : Component
    {
        List<T> results = new List<T>();

        // Lấy toàn bộ root GameObjects trong scene hiện tại
        var scene = SceneManager.GetActiveScene();
        var rootObjects = scene.GetRootGameObjects();

        foreach (var root in rootObjects)
        {
            results.AddRange(GetComponentsInChildrenIncludingInactive<T>(root));
        }

        return results;
    }

    private static List<T> GetComponentsInChildrenIncludingInactive<T>(GameObject obj) where T : Component
    {
        List<T> components = new List<T>();
        components.AddRange(obj.GetComponentsInChildren<T>(true)); // true để lấy cả inactive

        return components;
    }
}

public abstract class PoolObj : VuMonoBehaviour
{
    [SerializeField] protected DespawnBase despawn;
    public DespawnBase Despawn => despawn;

    public abstract string GetName();
    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadDespawn();
    }
    protected virtual void LoadDespawn()
    {
        if (this.despawn != null) return;
        this.despawn = transform.GetComponentInChildren<DespawnBase>();
        Debug.Log(transform.name + ": LoadDespawn",gameObject);
    }
}
