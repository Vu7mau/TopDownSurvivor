using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateParentObj : MonoBehaviour
{
    public static CreateParentObj instance;
    [SerializeField] private PowerUpSO listPowerUP;
    [SerializeField] private int amount;
    [SerializeField] private GameObject obj;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        CreateNewPowerUpParent();
    }
    public GameObject GetGem()
    {
        GameObject powerUp = null;
        Transform parentGems = transform.GetChild(0);
        if(parentGems != null && parentGems.childCount > 0)
        {
            foreach (Transform gem in parentGems)
            {
                if (!gem.gameObject.activeInHierarchy)
                {
                    return gem.gameObject;
                }
            }
        }
        if(powerUp == null)
        {
            Transform powerUp1 = Instantiate(listPowerUP.listPowerUps[0].transform);
            powerUp1.transform.parent = parentGems.transform;
            powerUp1.transform.gameObject.SetActive(false);
            return powerUp1.gameObject;
        }
        return null;
    }
    private void CreateNewPowerUpParent()
    {
        for(int j=0;j<listPowerUP.listPowerUps.Count;j++)
        {
            Transform parentPowerUp = Instantiate(obj.transform);
            parentPowerUp.parent = transform;
            parentPowerUp.gameObject.name = listPowerUP.listPowerUps[j].name;
            for (int i = 0; i < amount; i++)
            {
                GameObject powerUp = Instantiate(listPowerUP.listPowerUps[j]);
                powerUp.transform.parent = parentPowerUp.transform;
                powerUp.transform.gameObject.SetActive(false);
            }
        }
    }
}
