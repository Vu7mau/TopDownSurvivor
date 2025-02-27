using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

public class SetCoin : Singleton<SetCoin>
{    
    private TextMeshProUGUI Text_coin;      
    private int currentcoint;
    protected override  void Start()
    {
        currentcoint = 0;
        Text_coin = GetComponent<TextMeshProUGUI>();        
    }
    public void SetCoinToUI(int amount)
    {                
        currentcoint += amount;
        Text_coin.text = "x " + currentcoint.ToString();
    }    
    public void Savecoin()
    {                
        PlayerPrefs.SetInt("LastGameCoin", currentcoint);        
        PlayerPrefs.Save();        
    }

    protected override void OnDisable()
    {
        this.Savecoin();
    }
}
