using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditorInternal;
using UnityEngine;

public class TextCoinEnd : MonoBehaviour
{    
    private TextMeshProUGUI TextCoin;
    public SetCoin SetCoin;
    private void OnEnable()
    {
        TextCoin = GetComponent<TextMeshProUGUI>();         
        SetCoin.Savecoin();
        TextCoin.text = PlayerPrefs.GetInt("LastGameCoin").ToString();
    }    
}
