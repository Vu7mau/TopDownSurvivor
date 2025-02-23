using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class HpBar : MonoBehaviour
{
    private Slider SLider_Hpbar;
    private Coroutine currentCoroutine; 
    private Menu Menu;
    private void Start()
    {
        SLider_Hpbar = GetComponent<Slider>();
        Menu = FindObjectOfType<Menu>();
    }
    public void SetHealth(float hp)
    {        
        if (currentCoroutine != null )
        {
            StopCoroutine( currentCoroutine );

        }
        currentCoroutine = StartCoroutine(smoothHealth(hp));
    }       
    private IEnumerator smoothHealth(float targetHp)
    {
        float duration = 0.5f; 
        float elapsed = 0f; 
        float startHp = SLider_Hpbar.value; 

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            SLider_Hpbar.value = Mathf.Lerp(startHp, targetHp, elapsed / duration); 
            yield return null;
        }
        SLider_Hpbar.value = targetHp; 
        if (targetHp <= 0)
        {
            Menu.die();
        }
    }    
}
