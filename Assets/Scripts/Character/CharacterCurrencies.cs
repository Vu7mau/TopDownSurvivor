using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CharacterCurrencies : MonoBehaviour
{
    [SerializeField] protected int _coins = 0;

    [SerializeField] protected TMP_Text txt_coins_value;

    protected virtual void OnEnable()
    {
        if (txt_coins_value != null)
        {
            this.SetUI(this.txt_coins_value, this._coins);
        }
    }

    public virtual void AddCoins(int amount)
    {
        this._coins += amount;
        if(this.txt_coins_value != null)
        {
            this.SetUI(this.txt_coins_value, this._coins);
        }
        PlayerPrefs.SetInt("currentCoins",this._coins);
        PlayerPrefs.Save();
    }

    protected virtual void SetUI(TMP_Text text, int amount)
    {
        text.text = amount.ToString();
    }
}
