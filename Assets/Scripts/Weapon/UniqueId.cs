using UnityEngine;

[ExecuteAlways]
public class UniqueId : MonoBehaviour
{
    [SerializeField] private string id;
    public string Id
    {
        get
        {
            if (string.IsNullOrEmpty(id)) id = System.Guid.NewGuid().ToString("N");
            return id;
        }
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (string.IsNullOrEmpty(id)) id = System.Guid.NewGuid().ToString("N");
    }
#endif
}
