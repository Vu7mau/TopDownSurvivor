using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControllerAbstract : VuMonoBehaviour
{

    [Header("CharacterCtrl Abstract")]
    [SerializeField] protected GameController gameController;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadGameControllerAbstract();
    }
    protected virtual void LoadGameControllerAbstract()
    {
        if (this.gameController != null) return;
        gameController = this.transform.parent.GetComponent<GameController>();
    }

    protected virtual void LoadMap()
    {

    }    
}
