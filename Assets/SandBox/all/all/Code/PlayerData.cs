using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public static PlayerData Instance;
    public int coinCount = 0;
    public int hpPotionCount = 0;
    public TextMeshProUGUI textCoin;
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        coinCount = PlayerPrefs.GetInt("Coins", 0);
        hpPotionCount = PlayerPrefs.GetInt("HP", 0);
        UpdateCoinText();
    }
    public void AddCoin(int amount)
    {
        coinCount += amount;
        PlayerPrefs.SetInt("Coins",coinCount);
        PlayerPrefs.Save();
        UpdateCoinText();
    }
    public void AddHp(int amount)
    {
        hpPotionCount += amount;
    }
    private void UpdateCoinText()
    {
        if(textCoin != null)
        {
            textCoin.text = $"Coin: {coinCount}";
        }
        else
        {
            Debug.Log("Chưa gắn text coin");
        }
    }
}
