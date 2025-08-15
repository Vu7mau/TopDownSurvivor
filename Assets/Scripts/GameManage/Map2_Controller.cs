// File: Map2_Controller.cs
using UnityEngine;

public class Map2_Controller : Map_Controller
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameController.SwitchMap(mapIndex);
            CharacterCtrl.Instance.CharacterEffect.TurnOnLight();
        }
    }
}
