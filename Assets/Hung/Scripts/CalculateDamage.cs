using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCalculator : MonoBehaviour
{
    public static int CalculateDamage(int currentAtk, float currentCritRate, float currentCritDamage)
    {
        int baseDamage = currentAtk;
        float critMultiplier = 1f;

        // Xác suất chí mạng
        if (Random.Range(0f, 100f) < currentCritRate)
        {
            critMultiplier += currentCritDamage / 100f;
        }

        // Tính sát thương cuối cùng
        int finalDamage = Mathf.RoundToInt(baseDamage * critMultiplier);
        return finalDamage;
    }
}
