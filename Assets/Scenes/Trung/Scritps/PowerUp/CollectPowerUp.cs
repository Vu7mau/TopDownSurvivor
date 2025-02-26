using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectPowerUp : MonoBehaviour
{
    public static CollectPowerUp Instance { get; private set; }
    [SerializeField] private List<PowerUp> listPowerUp;

    private void Awake()
    {
        Instance = this;
    }
    public void AddPowerUpToArea(PowerUp _powerUp)
    {
        if(!listPowerUp.Contains(_powerUp))
        {
            listPowerUp.Add(_powerUp);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        CharacterCtrl player = other.GetComponent<CharacterCtrl>();
        if(player != null)
        {
            CollectAll(player);
        }
    }
    private void CollectAll(CharacterCtrl player)
    {
        foreach(var powerUp in listPowerUp)
        {
            powerUp.FollowPlayer(player);
        }
    }
    public void SetPositionForAreaCollectionPowerUp(Transform other)
    {
        transform.position = other.position;
    }
}
