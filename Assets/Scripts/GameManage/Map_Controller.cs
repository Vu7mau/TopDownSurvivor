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

    


    public  void EnableProcessing()
    {
        if (processing != null)
        {
            processing. gameObject.SetActive(true);
            processing.priority = 1f;
        }
    }
    public void DisableProcessing()
    {
        if (processing != null)
        {
            processing. gameObject.SetActive(false);
            processing.priority = 0f;
        }
    }





}
