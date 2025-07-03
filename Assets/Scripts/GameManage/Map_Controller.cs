using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Map_Controller : GameControllerAbstract
{

    // [SerializeField] public Transform map;

    // [SerializeField] private int mapIndex = 0;
    [Space]
    [Header("Map_Controller")]
    [SerializeField] public Transform currentMapSpawnPoint;
    [SerializeField] public Transform map;
    [SerializeField] protected Volume processing;
    [SerializeField] protected int mapIndexNextTo;   

    


    protected override void OnEnable()
    {
        if (processing != null)
        {
            processing.priority = 1f;
        }
    }
    protected override void OnDisable()
    {
        if (processing != null)
        {
            processing.priority = 0f;
        }
    }





}
