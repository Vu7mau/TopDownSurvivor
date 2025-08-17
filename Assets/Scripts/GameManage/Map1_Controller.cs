// File: Map1_Controller.cs
using UnityEngine;

public class Map1_Controller : Map_Controller
{
    private BoxCollider boxCol;

    protected override void Start()
    {
        base.Start();
        boxCol = GetComponentInChildren<BoxCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameController.SwitchMap(MapIndex);
            CharacterCtrl.Instance.CharacterEffect.TurnOnLight();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && boxCol)
        {
            // nếu muốn chặn quay lại:
            // boxCol.isTrigger = false;
        }
    }
}
