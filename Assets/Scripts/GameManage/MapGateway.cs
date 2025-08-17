// File: MapGateway.cs
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class MapGateway : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private bool useRelativeIndex = true;
    [SerializeField] private int relativeDelta = +1;
    [SerializeField] private int targetMapIndex = -1;

    [Header("Filter")]
    [SerializeField] private string playerTag = "Player";

    private GameController GC => GameController.Instance;

    private void Reset()
    {
        var col = GetComponent<Collider>();
        if (col) col.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(playerTag) || GC == null) return;

        int dest = useRelativeIndex
            ? Wrap(GC.CurrentMapIndex + relativeDelta, 0, GC.MapsCount - 1)
            : targetMapIndex;

        if (dest < 0 || dest >= GC.MapsCount) return;

        var name = MapNameOf(dest);
        ChatNotify.Instance?.MapSelected(dest, name);
        ChatNotify.Instance?.MapJumping(dest, name);
        GC.SwitchMap(dest);
     
    }

    private static int Wrap(int v, int min, int max)
    {
        int n = (max - min + 1);
        if (n <= 0) return min;
        v = (v - min) % n; if (v < 0) v += n; return v + min;
    }

    private string MapNameOf(int idx)
    {
        var maps = GC.transform.GetComponentsInChildren<Map_Controller>(true);
        foreach (var m in maps) if (m.MapIndex == idx) return m.map ? m.map.name : m.gameObject.name;
        return $"Map {idx}";
    }
}
