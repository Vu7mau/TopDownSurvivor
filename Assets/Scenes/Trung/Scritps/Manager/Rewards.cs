using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rewards : Singleton<Rewards>
{
    [SerializeField] private PowerUpSO listPowerUp;
    protected override void Awake()
    {
        base.Awake();
    }
    public void RewardGemsPlayerWhenKillEnemy(int amount,Transform enemy)
    {
        for(int i = 0; i < amount; i++)
        {
            Transform gems = CreateParentObj.instance.GetGem().transform;
            float x = Random.Range(enemy.position.x - 3f, enemy.position.x + 3f);
            float y = enemy.position.y + 5;
            float z = Random.Range(enemy.position.z - 3f, enemy.position.z + 3f);
            Vector3 position = new Vector3(x, y, z);
            gems.position = position;
            gems.gameObject.SetActive(true);
        }
    }
}
