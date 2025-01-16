using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCtrlAbstract : VuMonoBehaviour
{
    [Header("CharacterCtrl Abstract")]
    [SerializeField] protected CharacterCtrl _characterCtrl;

    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadCharacterCtrlAbstract();
    }
    protected virtual void LoadCharacterCtrlAbstract()
    {
        if (this._characterCtrl != null) return;
        _characterCtrl = this.transform.parent.GetComponent<CharacterCtrl>();
        Debug.Log("Load CharacterCtrl Abstract Success at " + this.transform.name);
    }
}
