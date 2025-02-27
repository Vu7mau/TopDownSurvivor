using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ProfileUIController : VuMonoBehaviour
{
    [SerializeField] protected TextMeshProUGUI Text_coin;

    
    protected override void OnEnable()
    {
        Text_coin.text=this.GetValueCoinPlayeref().ToString();


    }
    protected virtual int GetValueCoinPlayeref()
    {
        int value = PlayerPrefs.GetInt("LastGameCoin");
        return value;
    }
}
