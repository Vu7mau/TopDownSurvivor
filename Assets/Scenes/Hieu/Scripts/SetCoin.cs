using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

public class SetCoin : MonoBehaviour
{    
    private TextMeshProUGUI Text_coin;      
    private int currentcoint;
    private void Start()
    {
        currentcoint = 0;
        Text_coin = GetComponent<TextMeshProUGUI>();        
    }
    public void setCoin(int amount)
    {                
        currentcoint += amount;
        Text_coin.text = "x " + currentcoint.ToString();
    }    
    public void savecoin()
    {                
        PlayerPrefs.SetInt("LastGameCoin", currentcoint);        
        PlayerPrefs.Save();        
    }
}
