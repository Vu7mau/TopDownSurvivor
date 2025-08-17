// File: Map_Controller.cs
using UnityEngine;
using UnityEngine.Rendering;

public class Map_Controller : GameControllerAbstract
{
    [SerializeField] public Transform currentMapSpawnPoint;
    [SerializeField] public Transform map;
    [SerializeField] protected Volume processing;
    [SerializeField] private int mapIndex;

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

    public void SetMapIndex(int idx) { mapIndex = idx; }
}
