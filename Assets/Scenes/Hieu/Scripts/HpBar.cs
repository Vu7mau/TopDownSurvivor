using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class HpBar : MonoBehaviour
{
    [SerializeField]private Slider Hpbar;       
    public void SetHealth(float hp)
    {
        Hpbar.value = hp;
    }      
}
