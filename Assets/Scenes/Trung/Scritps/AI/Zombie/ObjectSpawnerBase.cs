using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ObjectSpawnerBase : VuMonoBehaviour
{
    [Header("Get the position when need ref the position spawner!")]
    [SerializeField] protected Transform position;

    [Header("Offset is a nesessary value when you need change the position (+/-)")]
    [SerializeField] protected Vector3 offSet;
}
