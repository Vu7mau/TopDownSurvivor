using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ItemPickUp : VuMonoBehaviour
{
    [SerializeField] private PickUpType _type;
    [SerializeField] private int _healingAmount;
    [SerializeField] private int _coinAmount;
    [SerializeField] private int _bulletAmount;
    protected override void Start()
    {
        base.Start();
        transform.DORotate(new Vector3(0, 360, 0), 2f, RotateMode.FastBeyond360)
    .SetLoops(-1, LoopType.Restart)
    .SetEase(Ease.Linear);

    }

    private void OnTriggerEnter(Collider other)
    {
        switch (_type)
        {
            case PickUpType.health:
                {
                    this.AddHealth(other);
                    break;
                }
            case PickUpType.ammount:
                {
                    this.AddAmmour(other);
                    break;
                }
        }
    }
    public void AddHealth(Collider other)
    {
        if(other.TryGetComponent<CharacterDamageReceiver>(out CharacterDamageReceiver damageReceiver))
        {
            damageReceiver.Add(_healingAmount);
            this.PlaySoundFXPickUpItem(snd_pickup);
            this.Despawn.DoDespawn();

        }
    }
    public void AddCoin(Collider other)
    {
        if (other.TryGetComponent<CharacterDamageReceiver>(out CharacterDamageReceiver damageReceiver))
        {
            CharacterCurrencies characterCurrencies = other.GetComponentInChildren<CharacterCurrencies>();
            if(characterCurrencies != null) characterCurrencies.AddCoins(this._coinAmount);
            this.PlaySoundFXPickUpItem(snd_pickup);
            this.Despawn.DoDespawn();

        }
    }
    public void AddAmmour(Collider other)
    {
        if(other.TryGetComponent<ActiveWeapon>(out ActiveWeapon activeWeapon))
        {
            if(activeWeapon.activeGun == null) return;
            activeWeapon.activeGun.UpdateTotalBullet(_bulletAmount);
            this.gameObject.SetActive(false);

        }

    }    

}
public enum PickUpType
{
    none = 0,
    health = 1,
    ammount = 2
}
