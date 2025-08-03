using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchTime : VuMonoBehaviour
{
    protected override void OnEnable()
    {
    CharacterCtrl.Instance.CharacterShooting.SetCancel(true);

    }
    protected override void OnDisable()
    {
        CharacterCtrl.Instance.CharacterShooting.SetCancel(false);

    }
}
