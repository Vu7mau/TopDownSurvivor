using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Magazine : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI AmmoText;   
    private MagazineData magazineData;
    private Coin coin;
    int a = 0;
    public void SetAmountAmmo(int magazine)
    {
        magazineData = new MagazineData(magazine);
    }
    private void Start()
    {
        SetAmountAmmo(600);
        coin = FindObjectOfType<Coin>();
    }
    public void fire(int amount)
    {        
        if (magazineData.UseAmmo(amount))
        {
            UpdateUI();
        }
        else
        {
            Debug.Log("Het dan");
        }                
    }
    public void reload()
    {
        magazineData.reload();
        UpdateUI();
    }
    private void UpdateUI()
    {
        AmmoText.text = $"{magazineData.CurrentAmmo}/{magazineData.MaxAmmo}";
    }
    private void Update()
    {
         fire(1);        
        coin.amountCoin(a++);
    }
}
