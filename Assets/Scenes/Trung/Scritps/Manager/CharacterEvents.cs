using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class CharacterEvents
{
    public static UnityAction<GameObject,GameObject, float> characterTookItem;
    public static UnityAction<GameObject, GameObject, float> characterTookExp;
    public static UnityAction<GameObject, float> characterDamaged;

    public static UnityAction<DamageSender> OnDamageSourceChanged;
    public static UnityAction<DamageSender> OnDamageSourceListChanged;
    public static Action OnCharacterPropertiesChanged;
}
