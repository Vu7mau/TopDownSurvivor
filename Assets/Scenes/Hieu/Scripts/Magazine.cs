using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Magazine : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI AmmoText;
    // private MagazineData magazineData;
    protected int _currentAmmour;
    protected int _maxAmmour;
    [SerializeField] protected int _currentGun;
    private RawImage ImageMagazine;
    public Texture[] ImageMagazines;

    bool isChange = false;

    private void Awake()
    {

        ImageMagazine = GetComponent<RawImage>();
        // AmmoText = GetComponent<TextMeshProUGUI>();
    }
    public void SetAmountAmmo(int current, int max)
    {
        this._currentAmmour = current;
        this._maxAmmour = max;
    }
    //public void reload()
    //{      
    //    UpdateUI();
    //}
    private void LateUpdate()
    {
        UpdateUI();
    }
    private void UpdateUI()
    {
        if (!CharacterCtrl.Instance.ActiveWeapon.activeGun) return;
        _currentAmmour = CharacterCtrl.Instance.ActiveWeapon.activeGun.GetCurrentAmmour();
        AmmoText.text = $"{_currentAmmour}/{_maxAmmour}";
        if (!CharacterCtrl.Instance.ActiveWeapon.IsHolstered)
        {
            if (isChange) return;
            _maxAmmour = CharacterCtrl.Instance.ActiveWeapon.activeGun.GetMaxBullets();
            ImageMagazine.texture = CharacterCtrl.Instance.ActiveWeapon.activeGun.GunTexture();
            this.isChange = true;
            Debug.Log("Do");
        }
        else this.isChange = false;




        
        //this.ChangeImageGun(this._currentGun);
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
