using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map2_Controller : Map_Controller
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameController.SwitchMap(mapIndex);
            this.processing.gameObject.SetActive(false);
            //CharacterUIManager.OnScreenFadeOut?.Invoke();
            //mapDisable.gameObject.SetActive(false);
            //Debug.Log("Enter map");
            CharacterCtrl.Instance.CharacterEffect.TurnOnLight();
        }
    }
}
