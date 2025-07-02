using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map1_Controller : Map_Controller
{


    private BoxCollider collider;
    protected override void Start()
    {
        collider=this.transform.GetComponentInChildren<BoxCollider>();
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameController.SwitchMap(1);
            //_map0.gameObject.SetActive(false);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            collider.isTrigger = false;
        }
    }

}
