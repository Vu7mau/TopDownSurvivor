using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class HpBar : MonoBehaviour
{
    public static HpBar Instance;
 [SerializeField]  private Slider SLider_Hpbar;
    private Coroutine currentCoroutine;
    private Menu Menu;
    bool isDead=false;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;


        SLider_Hpbar = GetComponent<Slider>();
    }
    private void Start()
    {
      
        Menu = FindObjectOfType<Menu>();

    }
    protected virtual void LateUpdate()
    {
        if (0 >= SLider_Hpbar.value&&!isDead)
        {
            Menu.die();
            isDead = true;
        }
    }
    public void SetHealth(float deductHp, float currentHp,float newMaxValue)
    {
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);

        }
        float tarGetHp = currentHp-deductHp;
 
        currentCoroutine = StartCoroutine(smoothHealth(tarGetHp, currentHp, newMaxValue));
    }
    private IEnumerator smoothHealth(float targetHp, float currentHp,float newMaxValue)
    {
        float duration = 0.75f;
        float elapsed = 0f;
        float startHp = currentHp;

       SLider_Hpbar.maxValue = newMaxValue;
        while (elapsed < duration)
        {
            
            elapsed += Time.deltaTime;
            float value = Mathf.Lerp(startHp, targetHp, elapsed / duration);
            SLider_Hpbar.value = value / 1;
            yield return null;
        }

        //SLider_Hpbar.value = deductHp; 
        
    }
    public virtual void SetHealthMaxBarVolume(float maxVolume)
    {
        SLider_Hpbar.maxValue = maxVolume;
        SLider_Hpbar.value = maxVolume;
    }
}
