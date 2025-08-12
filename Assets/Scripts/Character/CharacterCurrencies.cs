using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CharacterCurrencies : MonoBehaviour
{
    [SerializeField] protected int _coins = 0;
    [SerializeField] protected int _scores = 0;
    [SerializeField] protected int _kills = 0;

    [SerializeField] protected TMP_Text txt_coins_value;

    protected virtual void OnEnable()
    {
        if (txt_coins_value != null)
        {
            this.SetUI(this.txt_coins_value, this._coins);
        }
        this.ResetAllValues();
    }

    protected virtual void ResetAllValues()
    {
        PlayerPrefs.SetInt("currentCoins", 0);
        PlayerPrefs.SetInt("currentLevel", 1);
        PlayerPrefs.SetInt("currentKills", 0);
        PlayerPrefs.SetInt("currentScores", 0);
        PlayerPrefs.Save();
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

    public virtual void AddScore(int amount)
    {
        this._scores += amount;
        PlayerPrefs.SetInt("currentScores", this._scores);
        PlayerPrefs.Save();
    }

    public virtual void AddKills(int amount)
    {
        this._kills += amount;
        PlayerPrefs.SetInt("currentKills", this._kills);
        PlayerPrefs.Save();
    }

    protected virtual void SetUI(TMP_Text text, int amount)
    {
        text.text = amount.ToString();
    }
}
