using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField]private TextMeshProUGUI Text_coin;
    public void amountCoin(int amount)
    {
        Text_coin.text = "x "+amount.ToString();
    }    
}
