// File: Map_Controller.cs
using UnityEngine;
using UnityEngine.Rendering;

public class Map_Controller : GameControllerAbstract
{
    [SerializeField] public Transform currentMapSpawnPoint;
    [SerializeField] public Transform map;
    [SerializeField] protected Volume processing;
    [SerializeField] protected int mapIndex;

    public int MapIndex => mapIndex;

    public void EnableProcessing()
    {
        if (processing != null)
        {
            processing.gameObject.SetActive(true);
            processing.priority = 1f;
        }
    }
    public void DisableProcessing()
    {
        if (processing != null)
        {
            processing.gameObject.SetActive(false);
            processing.priority = 0f;
        }
    }

#if UNITY_EDITOR
    public void EditorSetMapIndex(int idx) { mapIndex = idx; }
#endif
}
