using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Magazine : MonoBehaviour
{
    private TextMeshProUGUI AmmoText;
    // private MagazineData magazineData;
    protected int _currentAmmour;
    protected int _maxAmmour;
    private RawImage ImageMagazine;
    public Texture[] ImageMagazines;
    private void Awake()
    {
        ImageMagazine = GetComponent<RawImage>();
        AmmoText = GetComponent<TextMeshProUGUI>();
    }
    public void SetAmountAmmo(int current,int max)
    {
        this._currentAmmour = current;
        this._maxAmmour = max;
    }     
    public void reload()
    {      
        UpdateUI();
    }
    //private void LateUpdate()
    //{
    //    UpdateUI();
    //}
    private void UpdateUI()
    {
        AmmoText.text = $"{_currentAmmour}/{_maxAmmour}";
    }  
    public void ChangeImageGun(int number)
    {
        for (int i = 0; i < ImageMagazines.Length; i++)
        {
            if (i == number)
            {
                ImageMagazine.texture = ImageMagazines[i];
                break;
            }
        }
    }
}
