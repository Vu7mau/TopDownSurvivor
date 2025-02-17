using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharacterEvents
{
    public static UnityAction<GameObject,GameObject, float> characterTookItem;
    public static UnityAction<GameObject, GameObject, float> characterTookExp;
    public static UnityAction<GameObject, float> characterDamaged;
}
